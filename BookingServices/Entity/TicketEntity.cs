using System;

public class Ticket : BaseEntity
{
    public Guid BookingId { get; set; }
    public Guid EventShowId { get; set; }
    public Guid? SeatId { get; set; }
    public Guid? TicketCategoryId { get; set; }
    public Guid OwnerUserId { get; set; }
    public string TicketNumber { get; set; } = null!;
    public string QRCode { get; set; } = null!;
    public byte Status { get; set; }
    public Guid? TransferredToUserId { get; set; }
    public DateTime? UsedAt { get; set; }

    public Booking Booking { get; set; } = null!;
    public EventShow EventShow { get; set; } = null!;
    public Seat? Seat { get; set; }
    public TicketCategory? TicketCategory { get; set; }
    public User OwnerUser { get; set; } = null!;
    public User? TransferredToUser { get; set; }
    public ICollection<TicketTransfer> TicketTransfers { get; set; } = new List<TicketTransfer>();
    public ICollection<TicketScan> TicketScans { get; set; } = new List<TicketScan>();
}
