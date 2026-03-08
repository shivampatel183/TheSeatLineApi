using TheSeatLineApi.Shared.Enums;

namespace TheSeatLineApi.Modules.BookingModule.Models.DTOs
{
    public class BookingResponseDto
    {
        public Guid Id { get; set; }
        public string BookingReference { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public string Status { get; set; } = null!;

        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ConvenienceFee { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "INR";

        public DateTime? HoldExpiresAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<BookingSeatDto> Seats { get; set; } = new();
    }
}



