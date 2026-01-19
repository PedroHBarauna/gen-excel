using GenExcel.Domain.Enums;

namespace GenExcel.Domain.Entities;

public class Sale
{
    public int SaleId { get; set; }

    public int TicketEventId { get; set; }
    public TicketEvent TicketEvent { get; set; } = null!;

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public string PurchaserName { get; set; } = null!;
    public string PurchaseEmail { get; set; } = null!;
    public string PurchaserCpf { get; set; } = null!;

    public int Amount { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime SaleDate { get; set; }

    public string PaymentForm { get; set; } = null!;
    public PaymentStatus PaymentStatus { get; set; }
}
