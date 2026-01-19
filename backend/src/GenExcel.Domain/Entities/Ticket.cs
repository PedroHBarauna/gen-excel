using GenExcel.Domain.Enums;

namespace GenExcel.Domain.Entities;

public class Ticket
{
    public int TicketId { get; set; }
    public string TicketType { get; set; } = null!;
    public string? Description { get; set; }
    public TicketCatalogStatus Status { get; set; } = TicketCatalogStatus.Active;
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public ICollection<TicketEvent> TicketEvents { get; set; } = new List<TicketEvent>();
}
