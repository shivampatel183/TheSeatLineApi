using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.BookingServices.DTOs
{
    public class CreateBookingRequestDto
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public List<Guid> SeatIds { get; set; } = new();

        public string? SpecialRequests { get; set; }
    }
}
