using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Tickets.Commands;
using GenExcel.Application.Tickets.Dtos;
using GenExcel.Application.Tickets.Queries;

namespace GenExcel.Application.Tickets.Services;

public interface ITicketEventService
{
    Task<PagedResult<TicketEventListDto>> SearchAsync(SearchTicketsEventQuery query, CancellationToken ct);
    Task<TicketEventDetailsDto?> GetByIdAsync(int ticketId, CancellationToken ct);
    Task<TicketEventDetailsDto> CreateAsync(CreateTicketEventRequest request, CancellationToken ct);
    Task<TicketEventDetailsDto?> UpdateAsync(int ticketId, UpdateTicketEventRequest request, CancellationToken ct);
    Task<bool> DeleteAsync(int ticketId, CancellationToken ct);
}
