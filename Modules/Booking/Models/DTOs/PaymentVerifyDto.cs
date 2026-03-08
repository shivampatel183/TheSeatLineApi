using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.BookingModule.Models.DTOs
{
    public class PaymentVerifyDto
    {
        [Required]
        public Guid BookingId { get; set; }

        [Required]
        public string RazorpayOrderId { get; set; } = null!;

        [Required]
        public string RazorpayPaymentId { get; set; } = null!;

        [Required]
        public string RazorpaySignature { get; set; } = null!;
    }
}



