using GenExcel.Application.Exports.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenExcel.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/reports")]
public sealed class EventReportController : ControllerBase
{
    private readonly IReportService _service;

    public EventReportController(IReportService service)
    {
        _service = service;
    }

    [HttpPost("spreadsheet")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> GenerateSpreadsheet(
        [FromBody] EventSpreadsheetReportRequest request,
        CancellationToken ct)
    {
        if (request.Reports is null || request.Reports.Count == 0)
            return BadRequest("Envie pelo menos 1 item em reports.");

        var bytes = await _service.GenerateBatchAsync(request, ct);

        var fileName = $"Relatorio_Teste.xlsx";
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var result = await _service.GetCategoriesAsync(ct);
        return Ok(result);
    }

    [HttpGet("texts/search")]
    public async Task<IActionResult> SearchTexts([FromQuery] string q, [FromQuery] int take = 10, CancellationToken ct = default)
    {
        var result = await _service.SearchTextsAsync(q, take, ct);
        return Ok(result);
    }

    [HttpGet("texts/{id:int}")]
    public async Task<IActionResult> GetTextDetails([FromRoute] int id, CancellationToken ct)
    {
        var details = await _service.GetTextoDetailsAsync(id, ct);
        return details is null ? NotFound() : Ok(details);
    }
}
