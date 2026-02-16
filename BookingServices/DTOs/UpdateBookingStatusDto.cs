using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.BookingServices.DTOs
{
    public class UpdateBookingStatusDto
    {
        [Required(ErrorMessage = "BookingStatus is required")]
        public BookingStatus BookingStatus { get; set; }

        [MaxLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters")]
        public string? CancellationReason { get; set; }
    }
}
