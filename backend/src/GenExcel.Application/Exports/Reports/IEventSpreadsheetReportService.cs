namespace GenExcel.Application.Exports.Reports;

public interface IEventSpreadsheetReportService
{
    Task<byte[]> GenerateBatchAsync(EventSpreadsheetReportRequest request, CancellationToken ct);
}
