using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Commands;

public sealed record CreateTicketEventRequest(
    int EventId,
    int TicketId,
    string Name,
    string? Description,
    decimal Price,
    int Available,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    TicketEventStatus Status
);
