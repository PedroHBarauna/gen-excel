using GenExcel.Domain.Enums;

namespace GenExcel.Application.Sales.Queries;

public sealed record SearchSalesQuery(
    int? EventId,
    int? TicketId,
    PaymentStatus? PaymentStatus,
    string? Email,
    string? Cpf,
    DateTime? FromUtc,
    DateTime? ToUtc,
    int Page = 1,
    int PageSize = 20,
    string Sort = "SaleDate",
    string Dir = "desc"
);
