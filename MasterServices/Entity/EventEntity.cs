public class Event : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid VenueId { get; set; }

    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }

    public byte EventType { get; set; }
    public Guid? CategoryId { get; set; }

    public string? Tags { get; set; }

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public string Timezone { get; set; } = null!;
    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; }

    public byte? AgeRestriction { get; set; }
    public string? Language { get; set; }

    public int MaxCapacity { get; set; }
    public byte Status { get; set; }

    public string? BannerImageUrl { get; set; }
    public Guid? CancellationPolicyId { get; set; }

    public string? Performers { get; set; }
    public bool IsDeleted { get; set; }

    public Venue Venue { get; set; } = null!;
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
