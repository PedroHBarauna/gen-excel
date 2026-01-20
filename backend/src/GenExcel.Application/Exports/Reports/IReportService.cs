namespace GenExcel.Application.Exports.Reports;

public interface IReportService
{
    Task<byte[]> GenerateBatchAsync(EventSpreadsheetReportRequest request, CancellationToken ct);
    Task<List<CategoryDto>> GetCategoriesAsync(CancellationToken ct);
    Task<List<SearchResultDto>> SearchTextsAsync(string q, int take, CancellationToken ct);
    Task<TextDetailsDto?> GetTextDetailsAsync(int eventId, CancellationToken ct);
}
