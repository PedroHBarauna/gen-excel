using GenExcel.Application.Exports.Reports;
using Microsoft.AspNetCore.Mvc;

namespace GenExcel.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class EventReportController : ControllerBase
{
    private readonly IEventSpreadsheetReportService _service;

    public EventReportController(IEventSpreadsheetReportService service)
    {
        _service = service;
    }

    [HttpPost("events")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> GenerateSpreadsheet(
        [FromBody] EventSpreadsheetReportRequest request,
        CancellationToken ct)
    {
        if (request.Reports is null || request.Reports.Count == 0)
            return BadRequest("Envie pelo menos 1 item em reports.");

        var bytes = await _service.GenerateBatchAsync(request, ct);

        var fileName = $"Relatorio_{DateTime.UtcNow:Mdyyyy}.xlsx";
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
}
