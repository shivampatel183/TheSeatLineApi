public class Payment : BaseEntity
{
    public Guid BookingId { get; set; }

    public string RazorpayOrderId { get; set; } = null!;
    public string? RazorpayPaymentId { get; set; }
    public string? RazorpaySignature { get; set; }

    public byte Method { get; set; }
    public byte Status { get; set; }

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";

    public decimal GatewayFee { get; set; }
    public decimal RefundAmount { get; set; }

    public string? RefundRazorpayId { get; set; }
    public DateTime? RefundedAt { get; set; }
    public string? RefundReason { get; set; }

    public string? InvoiceNumber { get; set; }
    public string? FailureReason { get; set; }
    public DateTime? WebhookReceivedAt { get; set; }

    public Booking Booking { get; set; } = null!;
}
