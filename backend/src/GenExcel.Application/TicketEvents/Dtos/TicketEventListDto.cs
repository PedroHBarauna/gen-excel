using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Dtos;

public sealed record TicketEventListDto(
    int TicketEventId,
    int TicketId,
    int EventId,
    string Name,
    string Description,
    decimal Price,
    int Available,
    int Sold,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    TicketEventStatus Status
);
