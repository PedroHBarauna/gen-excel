using ClosedXML.Excel;
using GenExcel.Application.Exports.Reports;
using GenExcel.Domain.Enums;
using GenExcel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.Infrastructure.Exports.Reports;

public sealed class ReportService : IReportService
{
    private readonly AppDbContext _db;

    private sealed record TicketEventRow(
        int EventId,
        string TicketType,
        decimal FeeRate,
        decimal Price,
        int Available,
        int Sold,
        DateTime SaleStartDate,
        DateTime SaleEndDate,
        TicketEventStatus Status,
        DateTime CreateDate,
        DateTime? UpdateDate
    );

    private enum ColFormat { None, Money, Integer, DateTime, Percent }

    private sealed record ColSpec(
        TicketEventColumn Col,
        string Header,
        Func<TicketEventRow, object?> Getter,
        ColFormat Format,
        Func<TicketEventRow, decimal>? Total
    );

    public ReportService(AppDbContext db) => _db = db;

    public async Task<byte[]> GenerateBatchAsync(EventSpreadsheetReportRequest request, CancellationToken ct)
    {
        ValidateRequest(request);

        var eventIds = request.Reports.Select(r => r.EventId).Distinct().ToList();

        var events = await _db.Event.AsNoTracking()
            .Where(e => eventIds.Contains(e.EventId))
            .ToListAsync(ct);

        if (events.Count != eventIds.Count)
            throw new InvalidOperationException("Um ou mais EventId não foram encontrados.");

        var evById = events.ToDictionary(e => e.EventId);

        var rows = await (
            from te in _db.TicketEvent.AsNoTracking()
            join t in _db.Ticket.AsNoTracking() on te.TicketId equals t.TicketId
            where eventIds.Contains(te.EventId)
            orderby te.EventId, t.TicketType
            select new TicketEventRow(
                te.EventId,
                t.TicketType,
                te.FeeRate,
                te.Price,
                te.Available,
                te.Sold,
                te.SaleStartDate,
                te.SaleEndDate,
                te.Status,
                te.CreateDate,
                te.UpdateDate
            )
        ).ToListAsync(ct);

        var byEventId = rows
            .GroupBy(r => r.EventId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Relatório 01");
        ws.Style.Font.FontName = "Aptos Narrow";
        ws.Style.Font.FontSize = 12;
        ws.Style.Fill.BackgroundColor = XLColor.White;
        ws.Style.Border.OutsideBorder = XLBorderStyleValues.None;
        ws.Style.Border.InsideBorder = XLBorderStyleValues.None;
        ws.Columns().Width = 17.5;
        ws.Rows().Height = 16;

        AddLogoIfExists(ws);

        ws.Cell("B6").Value = request.ReportTitle ?? "Teste geração arquivo Excel";
        ws.Cell("B8").Value = "Data/hora geração";
        ws.Cell("C8").Value = DateTime.UtcNow;
        ws.Cell("C8").Style.DateFormat.Format = "M/d/yyyy";

        var headerBg = XLColor.FromHtml("#0070C0");
        var headerFont = XLColor.White;
        var totalBg = XLColor.FromHtml("#7f7f7f");

        var tableIndex = 1;
        var labelRow = 10;

        foreach (var rep in request.Reports)
        {
            var ev = evById[rep.EventId];

            var ticketEvents = byEventId.TryGetValue(rep.EventId, out var arr)
                ? arr
                : Array.Empty<TicketEventRow>();

            ws.Cell(labelRow, 2).Value =
                $"{ev.EventName} — {ev.EventDateTime:dd/MM/yyyy} ({ev.City})";
            ws.Cell(labelRow, 2).Style.Font.Bold = true;

            var headerRow = labelRow + 2; 
            var dataStartRow = headerRow + 1;

            var specs = BuildSpecs(rep);

            ws.Cell(headerRow, 2).Value = "Ingresso";
            for (int i = 0; i < specs.Count; i++)
                ws.Cell(headerRow, 3 + i).Value = specs[i].Header;

            var headerRange = ws.Range(headerRow, 2, headerRow, 2 + specs.Count);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = headerFont;
            headerRange.Style.Fill.BackgroundColor = headerBg;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var totals = new decimal?[specs.Count];
            for (int i = 0; i < specs.Count; i++)
                totals[i] = specs[i].Total is null ? null : 0m;

            var r = dataStartRow;

            foreach (var te in ticketEvents)
            {
                var ticketCell = ws.Cell(r, 2);
                ticketCell.Value = te.TicketType;
                ticketCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                for (int i = 0; i < specs.Count; i++)
                {
                    var spec = specs[i];
                    var cell = ws.Cell(r, 3 + i);

                    SetCellValue(cell, spec.Getter(te));
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    if (spec.Total is not null)
                        totals[i] = (totals[i] ?? 0m) + spec.Total(te);
                }

                r++;
            }

            int blockEndRow;

            if (rep.IncludeTotal)
            {
                var totalRow = r;

                ws.Cell(totalRow, 2).Value = "Total";
                for (int i = 0; i < specs.Count; i++)
                {
                    var cell = ws.Cell(totalRow, 3 + i);
                    if (totals[i].HasValue) cell.SetValue(totals[i]!.Value);
                    else cell.Clear();
                }

                var totalRange = ws.Range(totalRow, 2, totalRow, 2 + specs.Count);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Font.FontColor = headerFont;
                totalRange.Style.Fill.BackgroundColor = totalBg;
                totalRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                blockEndRow = totalRow;

                ApplyColumnFormats(ws, specs, dataStartRow, totalRow);

                labelRow = totalRow + 2;
            }
            else
            {
                blockEndRow = (r - 1 >= dataStartRow) ? (r - 1) : headerRow;
                if (r - 1 >= dataStartRow)
                    ApplyColumnFormats(ws, specs, dataStartRow, r - 1);

                labelRow = blockEndRow + 2;
            }

            var blockRange = ws.Range(headerRow, 2, blockEndRow, 2 + specs.Count);
            blockRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            blockRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            blockRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            tableIndex++;
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    private static void ValidateRequest(EventSpreadsheetReportRequest request)
    {
        if (request.Reports is null || request.Reports.Count == 0)
            throw new InvalidOperationException("Envie pelo menos 1 item em reports.");

        if (request.Reports.Any(r => r.EventId <= 0))
            throw new InvalidOperationException("EventId inválido.");

        if (request.Reports.Select(r => r.EventId).Distinct().Count() != request.Reports.Count)
            throw new InvalidOperationException("reports contém EventId repetido.");

        foreach (var r in request.Reports)
        {
            if (r.Columns is null || r.Columns.Count is < 1 or > 12)
                throw new InvalidOperationException($"Columns deve ter de 1 a 12 itens (EventId={r.EventId}).");

            if (r.Columns.Distinct().Count() != r.Columns.Count)
                throw new InvalidOperationException($"Columns não pode conter itens repetidos (EventId={r.EventId}).");

            if (r.Headers is null || r.Headers.Count == 0)
                throw new InvalidOperationException($"Headers é obrigatório e não pode ser vazio (EventId={r.EventId}).");

            if (r.Headers.Count != r.Columns.Count)
                throw new InvalidOperationException($"Headers deve ter o mesmo tamanho de Columns (EventId={r.EventId}).");
        }

    }

    private static void AddLogoIfExists(IXLWorksheet ws)
    {
        var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "logo.png");
        if (!File.Exists(logoPath))
            return;

        ws.AddPicture(logoPath)
          .MoveTo(ws.Cell("B3"))
          .Scale(0.23);
    }

    private static List<ColSpec> BuildSpecs(EventReportItem rep)
    {
        var cols = rep.Columns;
        var headers = rep.Headers;

        var specs = new List<ColSpec>(cols.Count);

        for (int i = 0; i < cols.Count; i++)
        {
            var col = cols[i];
            var header = headers![i];

            specs.Add(col switch
            {
                TicketEventColumn.Price => new ColSpec(
                    col, header,
                    r => r.Price,
                    ColFormat.Money,
                    null
                ),

                TicketEventColumn.Available => new ColSpec(
                    col, header,
                    r => r.Available,
                    ColFormat.Integer,
                    r => r.Available
                ),

                TicketEventColumn.Sold => new ColSpec(
                    col, header,
                    r => r.Sold,
                    ColFormat.Integer,
                    r => r.Sold
                ),

                TicketEventColumn.SaleStartDate => new ColSpec(
                    col, header,
                    r => r.SaleStartDate,
                    ColFormat.DateTime,
                    null
                ),

                TicketEventColumn.SaleEndDate => new ColSpec(
                    col, header,
                    r => r.SaleEndDate,
                    ColFormat.DateTime,
                    null
                ),

                TicketEventColumn.Status => new ColSpec(
                    col, header,
                    r => r.Status.ToString(),
                    ColFormat.None,
                    null
                ),

                TicketEventColumn.CreateDate => new ColSpec(
                    col, header,
                    r => r.CreateDate,
                    ColFormat.DateTime,
                    null
                ),

                TicketEventColumn.UpdateDate => new ColSpec(
                    col, header,
                    r => r.UpdateDate,
                    ColFormat.DateTime,
                    null
                ),

                TicketEventColumn.FeeRate => new ColSpec(
                    col, header,
                    r => r.FeeRate, // 0.20m => 20%
                    ColFormat.Percent,
                    null
                ),

                TicketEventColumn.FeeValue => new ColSpec(
                    col, header,
                    r => r.Price * r.FeeRate,
                    ColFormat.Money,
                    r => r.Price * r.FeeRate
                ),

                _ => throw new InvalidOperationException($"Coluna não suportada: {col}")
            });
        }

        return specs;
    }

    private static void ApplyColumnFormats(IXLWorksheet ws, List<ColSpec> specs, int dataStartRow, int totalRow)
    {
        for (int i = 0; i < specs.Count; i++)
        {
            var spec = specs[i];
            var colIndex = 3 + i;

            var range = ws.Range(dataStartRow, colIndex, totalRow, colIndex);

            switch (spec.Format)
            {
                case ColFormat.Money:
                    range.Style.NumberFormat.Format = "R$ #,##0.00";
                    break;

                case ColFormat.Integer:
                    range.Style.NumberFormat.Format = "#,##0";
                    break;

                case ColFormat.Percent:
                    range.Style.NumberFormat.Format = "0%";
                    break;

                case ColFormat.DateTime:
                    range.Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                    break;

                case ColFormat.None:
                default:
                    break;
            }
        }
    }

    private static void SetCellValue(IXLCell cell, object? value)
    {
        if (value is null)
        {
            cell.Clear();
            return;
        }

        switch (value)
        {
            case string s:
                cell.SetValue(s);
                return;

            case int i:
                cell.SetValue(i);
                return;

            case long l:
                cell.SetValue(l);
                return;

            case decimal d:
                cell.SetValue(d);
                return;

            case double db:
                cell.SetValue(db);
                return;

            case float f:
                cell.SetValue((double)f);
                return;

            case bool b:
                cell.SetValue(b);
                return;

            case DateTime dt:
                cell.SetValue(dt);
                return;

            case DateTimeOffset dto:
                cell.SetValue(dto.UtcDateTime);
                return;

            default:
                cell.SetValue(value.ToString() ?? string.Empty);
                return;
        }
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(CancellationToken ct)
    {
        var cities = await _db.Event.AsNoTracking()
            .Select(e => e.City)
            .Where(c => c != null && c != "")
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(ct);

        return cities.Select(c => new CategoryDto(c, c)).ToList();
    }

    public async Task<List<SearchResultDto>> SearchTextsAsync(string q, int take, CancellationToken ct)
    {
        q = (q ?? "").Trim();
        if (q.Length == 0) return new();

        take = take is < 1 or > 50 ? 10 : take;

        var items = await _db.Event.AsNoTracking()
            .Where(e => e.EventName.Contains(q))
            .OrderBy(e => e.EventName)
            .Take(take)
            .Select(e => new SearchResultDto(e.EventId.ToString(), e.EventName))
            .ToListAsync(ct);

        return items;
    }

    public async Task<TextDetailsDto?> GetTextDetailsAsync(int eventId, CancellationToken ct)
    {
        var ev = await _db.Event.AsNoTracking()
            .Where(e => e.EventId == eventId)
            .Select(e => new { e.EventId, e.EventName, e.City })
            .FirstOrDefaultAsync(ct);

        if (ev is null) return null;

        var categoria = new CategoryDto(ev.City, ev.City);

        // Campos = TicketEventColumn (enum) com labels amigáveis
        var campos = GetCampos();

        return new TextDetailsDto(
            ev.EventId.ToString(),
            ev.EventName,
            categoria,
            campos
        );
    }

    private static List<FieldDto> GetCampos()
    {
        return new List<FieldDto>
        {
            new(nameof(TicketEventColumn.Available), "Disponível"),
            new(nameof(TicketEventColumn.Sold), "Vendido"),
            new(nameof(TicketEventColumn.Price), "Valor"),
            new(nameof(TicketEventColumn.FeeRate), "Taxa (%)"),
            new(nameof(TicketEventColumn.FeeValue), "Taxa (R$)"),
            new(nameof(TicketEventColumn.SaleStartDate), "Início Venda"),
            new(nameof(TicketEventColumn.SaleEndDate), "Fim Venda"),
            new(nameof(TicketEventColumn.Status), "Status"),
            new(nameof(TicketEventColumn.CreateDate), "Criado em"),
            new(nameof(TicketEventColumn.UpdateDate), "Atualizado em")
        };
    }

}
