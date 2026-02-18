public class Booking : BaseEntity
{
    public string BookingReference { get; set; } = null!;

    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }

    public byte Status { get; set; }
    public byte BookingSource { get; set; }

    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ConvenienceFee { get; set; }
    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = "INR";

    public DateTime? HoldExpiresAt { get; set; }
    public string? SpecialRequests { get; set; }

    public Guid? CancellationPolicyId { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }

    public bool IsDeleted { get; set; }

    public User User { get; set; } = null!;
    public Event Event { get; set; } = null!;

    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    public Payment? Payment { get; set; }
}
