using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Dtos;

public sealed record TicketEventDetailsDto(
    int TicketEventId,
    int TicketId,
    int EventId,
    string Name,
    string Description,
    decimal Price,
    decimal FeeRate,
    int Available,
    int Sold,
    DateTime SaleStartDate,
    DateTime SaleEndDate,
    TicketEventStatus Status,
    DateTime CreateDate,
    DateTime? UpdateDate
);
