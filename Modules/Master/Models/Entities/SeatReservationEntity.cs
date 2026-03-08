public class SeatReservation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SeatId { get; set; }
    public Guid EventShowId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public Seat Seat { get; set; } = null!;
    public EventShow EventShow { get; set; } = null!;
    public User User { get; set; } = null!;
}



