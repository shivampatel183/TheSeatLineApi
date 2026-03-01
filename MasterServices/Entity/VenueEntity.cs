public class Venue : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = null!;
    public byte VenueType { get; set; }

    public string? Description { get; set; }

    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public Guid CityId { get; set; }
    public string PostalCode { get; set; } = null!;

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public string Timezone { get; set; } = null!;
    public int TotalCapacity { get; set; }

    public string? Amenities { get; set; }
    public string? AccessibilityFeatures { get; set; }
    public string? MediaGallery { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? WebsiteUrl { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public City City { get; set; } = null!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<VenueOperatingHours> OperatingHours { get; set; } = new List<VenueOperatingHours>();
}
