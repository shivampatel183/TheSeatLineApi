using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.BookingServices.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int VenueId { get; set; }
        public int ShowId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Transfer Details
        public Guid? OriginalUserId { get; set; }
        public string? OriginalUserName { get; set; }
        public DateTime? TransferredAt { get; set; }
        public string? TransferNote { get; set; }

        // Expiry and Cancellation
        public DateTime? ExpiryTime { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        // Nested Objects
        public List<BookingSeatDto> Seats { get; set; } = new List<BookingSeatDto>();
        public VenueDetailDto Venue { get; set; }
        public ShowDetailDto Show { get; set; }
    }
}
