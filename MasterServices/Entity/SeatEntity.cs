using System.ComponentModel.DataAnnotations;

public class Seat : BaseEntity
{
    public Guid EventId { get; set; }

    public string? Section { get; set; }
    public string? Row { get; set; }
    public string SeatNumber { get; set; } = null!;

    public byte SeatType { get; set; }
    public byte Status { get; set; }

    public decimal BasePrice { get; set; }
    public string Currency { get; set; } = "INR";

    public bool IsAisle { get; set; }
    public bool IsWindow { get; set; }
    public bool IsEmergencyExit { get; set; }

    public Guid? GroupId { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public Guid EventShowId { get; set; }

    public Event Event { get; set; } = null!;
    public EventShow EventShow { get; set; } = null!;
}
