using TheSeatLineApi.Data.Entities;
using TheSeatLineApi.Modules.AuthModule.Models.Entities;

public class Event : BaseEntity
{
    public Guid VenueId { get; set; }
    public Guid? CategoryId { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public byte EventType { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; } 
    public string Timezone { get; set; } = null!;
    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; }
    public byte? AgeRestriction { get; set; }
    public string? Language { get; set; }
    public int MaxCapacity { get; set; }
    public byte Status { get; set; }
    public string? BannerImageUrl { get; set; }
    public Guid? CancellationPolicyId { get; set; }
    public string? Performers { get; set; }   // Stored as JSON string
    public bool IsDeleted { get; set; }

    // Navigation properties
    public Venue Venue { get; set; } = null!;
    public EventCategory? Category { get; set; }

    // Collections
    public ICollection<EventShow> Shows { get; set; } = new List<EventShow>();
    public ICollection<EventImage> Images { get; set; } = new List<EventImage>();
    public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
    public ICollection<Coupon> CouponEvents { get; set; } = new List<Coupon>();
    public Guid OrganizerId { get; set; }
    public User Organizer { get; set; } = null!;
}


