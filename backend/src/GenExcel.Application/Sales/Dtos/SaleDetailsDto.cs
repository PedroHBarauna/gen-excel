using GenExcel.Domain.Enums;

namespace GenExcel.Application.Sales.Dtos;

public sealed record SaleDetailsDto(
    int SaleId,
    int TicketId,
    int EventId,
    string PurchaserName,
    string PurchaseEmail,
    string PurchaserCpf,
    int Amount,
    decimal TotalPrice,
    DateTime SaleDate,
    string PaymentForm,
    PaymentStatus PaymentStatus
);
