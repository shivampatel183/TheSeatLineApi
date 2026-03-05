public class TicketScan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TicketId { get; set; }
    public DateTime ScannedAt { get; set; } = DateTime.UtcNow;
    public Guid? ScannedBy { get; set; }

    public Ticket Ticket { get; set; } = null!;
}
