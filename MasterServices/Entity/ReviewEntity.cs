public class Review : BaseEntity
{
    public Guid? EventId { get; set; }
    public Guid? VenueId { get; set; }

    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }

    public byte Rating { get; set; }

    public string? Title { get; set; }
    public string? Body { get; set; }

    public bool IsVerified { get; set; }
    public int HelpfulVotes { get; set; }

    public byte ModerationStatus { get; set; }

    public string? VenueResponse { get; set; }
    public DateTime? VenueResponseAt { get; set; }

    public bool IsDeleted { get; set; }

    public User User { get; set; } = null!;
    public Booking Booking { get; set; } = null!;
}
