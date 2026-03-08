using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Shared.Enums;

namespace TheSeatLineApi.Modules.BookingModule.Models.DTOs
{
    public class UpdateBookingStatusDto
    {
        [Required(ErrorMessage = "BookingStatus is required")]
        public BookingStatus BookingStatus { get; set; }

        [MaxLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters")]
        public string? CancellationReason { get; set; }
    }
}



