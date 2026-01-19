using GenExcel.Domain.Enums;

namespace GenExcel.Application.Sales.Commands;

public sealed record UpdateSaleRequest(
    PaymentStatus PaymentStatus
);
