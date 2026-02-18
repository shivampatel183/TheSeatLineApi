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

    public byte[] RowVersion { get; set; } = null!;

    public Event Event { get; set; } = null!;
}
