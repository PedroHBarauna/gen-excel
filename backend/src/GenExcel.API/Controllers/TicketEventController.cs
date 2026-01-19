using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Commands;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Queries;
using GenExcel.Application.Tickets.Services;
using GenExcel.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.API.Controllers;

[ApiController]
[Route("api/tickets")]
public sealed class TicketEventController : ControllerBase
{
    private readonly ITicketEventService _service;

    public TicketEventController(ITicketEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TicketEventListDto>>> Search(
        [FromQuery] int? eventId = null,
        [FromQuery] int? ticketId = null,
        [FromQuery] string? search = null,
        [FromQuery] TicketEventStatus? status = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] bool onSaleNow = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sort = "SaleStartDate",
        [FromQuery] string dir = "asc",
        CancellationToken ct = default)
    {
        var query = new SearchTicketsEventQuery(
            EventId: eventId,
            TicketId: ticketId,
            Search: search,
            Status: status,
            MinPrice: minPrice,
            MaxPrice: maxPrice,
            OnSaleNow: onSaleNow,
            Page: page,
            PageSize: pageSize,
            Sort: sort,
            Dir: dir
        );

        var result = await _service.SearchAsync(query, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketEventDetailsDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TicketEventDetailsDto>> Create([FromBody] CreateTicketEventRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.TicketId }, created);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<TicketEventDetailsDto>> Update(
        [FromRoute] int id,
        [FromBody] UpdateTicketEventRequest request,
        CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, request, ct);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "Cannot delete ticket because it has related sales." });
        }
    }
}
