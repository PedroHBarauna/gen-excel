using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Events.Dtos;
using GenExcel.Application.Events.Queries;
using GenExcel.Domain.Entities;

namespace GenExcel.Application.Events.Ports;

public interface IEventRepository
{
    Task<PagedResult<EventListDto>> SearchAsync(SearchEventsQuery query, CancellationToken ct);
    Task<Event?> GetByIdAsync(int eventId, CancellationToken ct);
    Task AddAsync(Event entity, CancellationToken ct);
    void Update(Event entity);
    void Remove(Event entity);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
