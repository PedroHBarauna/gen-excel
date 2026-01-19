using GenExcel.Domain.Enums;

namespace GenExcel.Domain.Entities;

public class TicketEvent
{
    public int TicketEventId { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
    public int Available { get; set; }
    public int Sold { get; set; }

    public DateTime SaleStartDate { get; set; }
    public DateTime SaleEndDate { get; set; }

    public TicketEventStatus Status { get; set; } = TicketEventStatus.Available;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
