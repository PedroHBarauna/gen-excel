using GenExcel.Domain.Enums;

namespace GenExcel.Domain.Entities;

public class Event
{
    public int EventId { get; set; }
    public string EventName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime EventDateTime { get; set; }
    public string City { get; set; } = null!;
    public int TotalCapacity { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Active;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public ICollection<TicketEvent> TicketEvents { get; set; } = new List<TicketEvent>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
