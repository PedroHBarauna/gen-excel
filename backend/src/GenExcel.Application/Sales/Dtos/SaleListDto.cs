using GenExcel.Domain.Enums;

namespace GenExcel.Application.Sales.Dtos;

public sealed record SaleListDto(
    int SaleId,
    int EventId,
    int TicketId,
    string PurchaserName,
    string PurchaseEmail,
    int Amount,
    decimal TotalPrice,
    DateTime SaleDate,
    string PaymentForm,
    PaymentStatus PaymentStatus
);
