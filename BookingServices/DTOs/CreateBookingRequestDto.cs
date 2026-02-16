using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.BookingServices.DTOs
{
    public class CreateBookingRequestDto
    {
        [Required(ErrorMessage = "ShowId is required")]
        public int ShowId { get; set; }

        [Required(ErrorMessage = "ShowSeatCategoryId is required")]
        public int ShowSeatCategoryId { get; set; }

        [Required(ErrorMessage = "NumberOfSeats is required")]
        [Range(1, 10, ErrorMessage = "Number of seats must be between 1 and 10")]
        public int NumberOfSeats { get; set; }
    }
}
