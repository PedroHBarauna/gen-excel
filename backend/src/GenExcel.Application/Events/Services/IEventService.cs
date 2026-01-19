using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Events.Commands;
using GenExcel.Application.Events.Dtos;
using GenExcel.Application.Events.Queries;

namespace GenExcel.Application.Events.Services;

public interface IEventService
{
    Task<PagedResult<EventListDto>> SearchAsync(SearchEventsQuery query, CancellationToken ct);
    Task<EventDetailsDto?> GetByIdAsync(int eventId, CancellationToken ct);
    Task<EventDetailsDto> CreateAsync(CreateEventRequest request, CancellationToken ct);
    Task<EventDetailsDto?> UpdateAsync(int eventId, UpdateEventRequest request, CancellationToken ct);
    Task<bool> DeleteAsync(int eventId, CancellationToken ct);
}
