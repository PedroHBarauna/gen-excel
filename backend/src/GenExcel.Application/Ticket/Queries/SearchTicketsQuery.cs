using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Queries;

public sealed record SearchTicketsQuery(
    string? TicketType,
    TicketCatalogStatus? Status,
    int Page = 1,
    int PageSize = 20,
    string Sort = "TicketId",
    string Dir = "asc"
);
