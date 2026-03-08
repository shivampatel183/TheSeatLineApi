using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("PaymentRefunds")]
    public class PaymentRefund
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PaymentId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The refund ID returned by the payment gateway (e.g., Razorpay refund ID).
        /// </summary>
        [MaxLength(100)]
        public string? RefundGatewayId { get; set; }

        /// <summary>
        /// Reason provided for the refund.
        /// </summary>
        [MaxLength(500)]
        public string? Reason { get; set; }

        /// <summary>
        /// Status of the refund: 0 = Initiated, 1 = Completed, 2 = Failed.
        /// </summary>
        public byte Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(PaymentId))]
        public virtual Payment Payment { get; set; } = null!;
    }
}



