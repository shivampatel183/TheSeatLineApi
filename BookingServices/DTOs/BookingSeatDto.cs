namespace TheSeatLineApi.BookingServices.DTOs
{
    public class BookingSeatDto
    {
        public int Id { get; set; }
        public Guid BookingId { get; set; }
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatType { get; set; }
        public decimal Price { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
    }
}
