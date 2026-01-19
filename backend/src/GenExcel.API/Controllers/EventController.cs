using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Events.Commands;
using GenExcel.Application.Events.Dtos;
using GenExcel.Application.Events.Queries;
using GenExcel.Application.Events.Services;
using GenExcel.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenExcel.API.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<EventListDto>>> Search(
        [FromQuery] string? search = null,
        [FromQuery] string? city = null,
        [FromQuery] EventStatus? status = null,
        [FromQuery] DateTime? fromUtc = null,
        [FromQuery] DateTime? toUtc = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sort = "EventDateTime",
        [FromQuery] string dir = "asc",
        CancellationToken ct = default)
    {
        var query = new SearchEventsQuery(search, city, status, fromUtc, toUtc, page, pageSize, sort, dir);
        var result = await _service.SearchAsync(query, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EventDetailsDto>> Create([FromBody] CreateEventRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.EventId }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDetailsDto>> GetById([FromRoute] int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<EventDetailsDto>> Update(
        [FromRoute] int id,
        [FromBody] UpdateEventRequest request,
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
            return Conflict(new { message = "Cannot delete event because it has related sales/tickets." });
        }
    }
}
