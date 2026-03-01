namespace TheSeatLineApi.BookingServices.DTOs
{
    public class BookingSeatDto
    {
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; } = null!;
        public string SeatType { get; set; } = null!;
        public string Row { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
