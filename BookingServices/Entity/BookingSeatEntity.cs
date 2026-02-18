public class BookingSeat : BaseEntity
{
    public Guid BookingId { get; set; }
    public Guid SeatId { get; set; }

    public decimal PriceAtBooking { get; set; }

    public Booking Booking { get; set; } = null!;
    public Seat Seat { get; set; } = null!;
}
