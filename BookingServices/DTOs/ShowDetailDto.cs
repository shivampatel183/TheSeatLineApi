namespace TheSeatLineApi.BookingServices.DTOs
{
    public class ShowDetailDto
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; }
        public int Duration { get; set; }
        public string? Genre { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
    }
}
