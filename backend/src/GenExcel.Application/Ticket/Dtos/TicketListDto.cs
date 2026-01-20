using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Dtos;

public sealed record TicketListDto(
    int TicketId,
    string TicketType,
    string? Description,
    TicketCatalogStatus Status,
    DateTime CreateDate,
    DateTime? UpdateDate
);
