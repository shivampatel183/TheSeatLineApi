namespace TheSeatLineApi.BookingServices.DTOs
{
    public class VenueDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int Capacity { get; set; }
    }
}
