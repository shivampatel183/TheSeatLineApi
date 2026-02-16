using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.BookingServices.DTOs
{
    public class BookingResponseDto
    {
        public Guid BookingId { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        // Transfer Details
        public Guid? OriginalUserId { get; set; }
        public string? OriginalUserName { get; set; }
        public DateTime? TransferredAt { get; set; }
        public string? TransferNote { get; set; }

        // Show Details
        public int ShowId { get; set; }
        public DateTime ShowTime { get; set; }

        // Event Details
        public int EventId { get; set; }
        public string EventName { get; set; }

        // Venue Details
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string VenueCity { get; set; }

        // Seat Category Details
        public int ShowSeatCategoryId { get; set; }
        public string SeatCategoryName { get; set; }
        public decimal PricePerSeat { get; set; }
    }
}
