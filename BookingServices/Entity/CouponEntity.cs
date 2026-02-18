public class Coupon : BaseEntity
{
    public Guid TenantId { get; set; }

    public string Code { get; set; } = null!;
    public string? Description { get; set; }

    public byte DiscountType { get; set; }
    public decimal DiscountValue { get; set; }

    public decimal? MaxDiscount { get; set; }
    public decimal? MinPurchaseAmt { get; set; }

    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }

    public int? TotalUsageLimit { get; set; }
    public int? PerUserLimit { get; set; }

    public int UsedCount { get; set; }
    public string? ApplicableEventIds { get; set; }

    public bool IsActive { get; set; }
}
