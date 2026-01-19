using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Commands;

public sealed record UpdateTicketEventRequest(
    string Name,
    string Description,
    decimal Price,
    int Available,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    TicketEventStatus Status
);
