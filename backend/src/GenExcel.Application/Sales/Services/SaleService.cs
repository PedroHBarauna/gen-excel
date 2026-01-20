using GenExcel.Application.Common.Pagination;
using GenExcel.Application.Sales.Commands;
using GenExcel.Application.Sales.Dtos;
using GenExcel.Application.Sales.Ports;
using GenExcel.Application.Sales.Queries;
using GenExcel.Application.Tickets.Ports;
using GenExcel.Domain.Entities;
using GenExcel.Domain.Enums;

namespace GenExcel.Application.Sales.Services;

public sealed class SaleService : ISaleService
{
    private readonly ISaleRepository _sales;
    private readonly ITicketEventRepository _tickets;

    public SaleService(ISaleRepository sales, ITicketEventRepository tickets)
    {
        _sales = sales;
        _tickets = tickets;
    }

    public Task<PagedResult<SaleListDto>> SearchAsync(SearchSalesQuery query, CancellationToken ct)
        => _sales.SearchAsync(query, ct);

    public async Task<SaleDetailsDto?> GetByIdAsync(int saleId, CancellationToken ct)
    {
        var sale = await _sales.GetByIdAsync(saleId, ct);
        return sale is null ? null : Map(sale);
    }

    public async Task<SaleDetailsDto> CreateAsync(CreateSaleRequest request, CancellationToken ct)
    {
        Validate(request);

        var ticket = await _tickets.GetByIdAsync(request.TicketEventId, ct)
            ?? throw new InvalidOperationException("Ticket not found.");

        if (ticket.EventId != request.EventId)
            throw new InvalidOperationException("Ticket does not belong to the provided EventId.");

        if (request.Amount > ticket.Available)
            throw new InvalidOperationException("Not enough tickets available.");

        var now = DateTime.UtcNow;

        var sale = new Sale
        {
            TicketEventId = request.TicketEventId,
            EventId = request.EventId,
            PurchaserName = request.PurchaserName.Trim(),
            PurchaseEmail = request.PurchaseEmail.Trim(),
            PurchaserCpf = request.PurchaserCpf.Trim(),
            Amount = request.Amount,
            TotalPrice = request.Amount * ticket.Price,
            SaleDate = now,
            PaymentForm = request.PaymentForm.Trim(),
            PaymentStatus = PaymentStatus.Pending
        };

        await _sales.AddAsync(sale, ct);

        if (sale.Amount > ticket.Available)
            throw new InvalidOperationException("Not enough tickets available.");

            ticket.Available -= sale.Amount;
            ticket.Sold += sale.Amount;
            _tickets.Update(ticket);
        

        await _sales.SaveChangesAsync(ct);

        return Map(sale);
    }

    public async Task<SaleDetailsDto?> UpdateAsync(int saleId, UpdateSaleRequest request, CancellationToken ct)
    {
        var sale = await _sales.GetByIdAsync(saleId, ct);
        if (sale is null) return null;

        var ticket = await _tickets.GetByIdAsync(sale.TicketEventId, ct)
            ?? throw new InvalidOperationException("Ticket not found for this sale.");

        var oldStatus = sale.PaymentStatus;
        var oldAmount = sale.Amount;
        sale.PaymentStatus = request.PaymentStatus;

        AdjustInventoryOnStatusChange(ticket, oldStatus, sale.PaymentStatus, sale.Amount);

        _sales.Update(sale);
        _tickets.Update(ticket);

        await _sales.SaveChangesAsync(ct);

        return Map(sale);
    }

    private static void AdjustInventoryOnStatusChange(
        Domain.Entities.TicketEvent ticket,
        PaymentStatus oldStatus,
        PaymentStatus newStatus,
        int amount)
    {
        var oldReserves = oldStatus is PaymentStatus.Pending or PaymentStatus.Approved;
        var newReserves = newStatus is PaymentStatus.Pending or PaymentStatus.Approved;

        if (oldReserves == newReserves)
            return;

        if (!oldReserves && newReserves)
        {
            if (amount > ticket.Available)
                throw new InvalidOperationException("Not enough tickets available.");

            ticket.Available -= amount;
            ticket.Sold += amount;
            return;
        }

        ticket.Available += amount;
        ticket.Sold -= amount;
        if (ticket.Sold < 0) ticket.Sold = 0;
    }

    private static void Validate(CreateSaleRequest r)
    {
        if (r.TicketEventId <= 0) throw new ArgumentException("TicketId is required.");
        if (r.EventId <= 0) throw new ArgumentException("EventId is required.");
        if (string.IsNullOrWhiteSpace(r.PurchaserName)) throw new ArgumentException("PurchaserName is required.");
        if (string.IsNullOrWhiteSpace(r.PurchaseEmail)) throw new ArgumentException("PurchaseEmail is required.");
        if (string.IsNullOrWhiteSpace(r.PurchaserCpf)) throw new ArgumentException("PurchaserCpf is required.");
        if (r.Amount <= 0) throw new ArgumentException("Amount must be > 0.");
        if (string.IsNullOrWhiteSpace(r.PaymentForm)) throw new ArgumentException("PaymentForm is required.");
    }

    private static SaleDetailsDto Map(Sale s) =>
        new(
            s.SaleId,
            s.TicketEventId,
            s.EventId,
            s.PurchaserName,
            s.PurchaseEmail,
            s.PurchaserCpf,
            s.Amount,
            s.TotalPrice,
            s.SaleDate,
            s.PaymentForm,
            s.PaymentStatus
        );
}
