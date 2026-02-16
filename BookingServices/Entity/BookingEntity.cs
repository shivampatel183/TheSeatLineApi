using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSeatLineApi.AuthServices.Entity;
using TheSeatLineApi.Common.Enums;
using TheSeatLineApi.Entity;

namespace TheSeatLineApi.BookingServices.Entity
{
    public class BookingEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int ShowId { get; set; }

        [Required]
        public int ShowSeatCategoryId { get; set; }

        [Required]
        [Range(1, 10)]
        public int NumberOfSeats { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryTime { get; set; }

        public DateTime? ConfirmedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        [MaxLength(500)]
        public string? CancellationReason { get; set; }

        // Transfer tracking
        public Guid? OriginalUserId { get; set; }

        public DateTime? TransferredAt { get; set; }

        [MaxLength(500)]
        public string? TransferNote { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ShowId")]
        public ShowEntity Show { get; set; }

        [ForeignKey("ShowSeatCategoryId")]
        public ShowSeatCategoryEntity ShowSeatCategory { get; set; }
    }
}
