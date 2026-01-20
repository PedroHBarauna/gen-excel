using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Queries;
using GenExcel.Domain.Entities;

namespace GenExcel.Application.Tickets.Ports;

public interface ITicketEventRepository
{
    Task<PagedResult<TicketEventListDto>> SearchAsync(SearchTicketsEventQuery query, CancellationToken ct);
    Task<TicketEvent?> GetByIdAsync(int ticketId, CancellationToken ct);
    Task AddAsync(TicketEvent entity, CancellationToken ct);
    void Update(TicketEvent entity);
    void Remove(TicketEvent entity);
    Task<int> SaveChangesAsync(CancellationToken ct);
}

