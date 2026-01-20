using GenExcel.Domain.Enums;

namespace GenExcel.Application.Tickets.Queries;

public sealed record SearchTicketsEventQuery(
    int? EventId,
    int? TicketId,
    string? Search,
    TicketEventStatus? Status,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool OnSaleNow = false,
    int Page = 1,
    int PageSize = 20,
    string Sort = "SaleStartDate",
    string Dir = "asc"
);
