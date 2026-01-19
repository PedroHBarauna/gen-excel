using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Sales.Commands;
using GenExcel.Application.Sales.Dtos;
using GenExcel.Application.Sales.Queries;
using GenExcel.Application.Sales.Services;
using GenExcel.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.API.Controllers;

[ApiController]
[Route("api/sales")]
public sealed class SalesController : ControllerBase
{
    private readonly ISaleService _service;

    public SalesController(ISaleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SaleListDto>>> Search(
        [FromQuery] int? eventId = null,
        [FromQuery] int? ticketId = null,
        [FromQuery] PaymentStatus? paymentStatus = null,
        [FromQuery] string? email = null,
        [FromQuery] string? cpf = null,
        [FromQuery] DateTime? fromUtc = null,
        [FromQuery] DateTime? toUtc = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sort = "SaleDate",
        [FromQuery] string dir = "desc",
        CancellationToken ct = default)
    {
        var query = new SearchSalesQuery(
            EventId: eventId,
            TicketId: ticketId,
            PaymentStatus: paymentStatus,
            Email: email,
            Cpf: cpf,
            FromUtc: fromUtc,
            ToUtc: toUtc,
            Page: page,
            PageSize: pageSize,
            Sort: sort,
            Dir: dir
        );

        var result = await _service.SearchAsync(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SaleDetailsDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDetailsDto>> Create([FromBody] CreateSaleRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.SaleId }, created);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<SaleDetailsDto>> Update(
        [FromRoute] int id,
        [FromBody] UpdateSaleRequest request,
        CancellationToken ct)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, request, ct);
            if (updated is null) return NotFound();
            return Ok(updated);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "Database constraint conflict while updating sale." });
        }
    }
}
