using GenExcel.Domain.Enums;

namespace GenExcel.Application.Sales.Commands;

public sealed record CreateSaleRequest(
    int TicketEventId,
    int EventId,
    string PurchaserName,
    string PurchaseEmail,
    string PurchaserCpf,
    int Amount,
    string PaymentForm
);
