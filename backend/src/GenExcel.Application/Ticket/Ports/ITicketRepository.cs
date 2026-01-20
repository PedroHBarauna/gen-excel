using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Queries;
using GenExcel.Domain.Entities;

namespace GenExcel.Application.Tickets.Ports;

public interface ITicketRepository
{
    Task<PagedResult<TicketListDto>> SearchAsync(SearchTicketsQuery query, CancellationToken ct);
    Task<Ticket?> GetByIdAsync(int ticketId, CancellationToken ct);
    Task AddAsync(Ticket entity, CancellationToken ct);
    void Update(Ticket entity);
    void Remove(Ticket entity);
    Task<int> SaveChangesAsync(CancellationToken ct);
}

