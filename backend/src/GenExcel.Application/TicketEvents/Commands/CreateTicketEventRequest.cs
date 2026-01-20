using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Commands;

public sealed record CreateTicketEventRequest(
    int EventId,
    int TicketId,
    string Name,
    string? Description,
    decimal FeeRate,
    decimal Price,
    int Available,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    TicketEventStatus Status
);
