public class BookingItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public Guid? TicketCategoryId { get; set; }
    public Guid? SeatId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Booking Booking { get; set; } = null!;
    public TicketCategory? TicketCategory { get; set; }
    public Seat? Seat { get; set; }
}



