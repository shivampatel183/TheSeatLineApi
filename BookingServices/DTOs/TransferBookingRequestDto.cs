using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.BookingServices.DTOs
{
    public class TransferBookingRequestDto
    {
        [Required(ErrorMessage = "Recipient email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string RecipientEmail { get; set; }

        [MaxLength(500, ErrorMessage = "Transfer note cannot exceed 500 characters")]
        public string? TransferNote { get; set; }
    }
}
