using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Modules.AuthModule.Models.Entities;
using TheSeatLineApi.Modules.BookingModule.Models.Entities;
using TheSeatLineApi.Modules.MasterModule.Models.Entities;

[Table("Users")]
public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [MaxLength(512)]
    public string? PasswordHash { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(1024)]
    public string? AvatarUrl { get; set; }

    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool MfaEnabled { get; set; }
    public string? MfaSecret { get; set; }

    [MaxLength(50)]
    public string? OAuthProvider { get; set; }

    [MaxLength(256)]
    public string? OAuthId { get; set; }

    [MaxLength(512)]
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public DateTime? LastLoginAt { get; set; }
    public byte FailedLoginCount { get; set; }
    public DateTime? LockoutUntil { get; set; }

    /// <summary>
    /// 1 = Customer, 2 = Organizer, 3 = Admin
    /// </summary>
    public int UserType { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Navigation properties (incoming)

    /// <summary>
    /// Events organized by this user (if UserType = Organizer).
    /// </summary>
    public virtual ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();

    /// <summary>
    /// Bookings made by this user (if UserType = Customer).
    /// </summary>
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    /// <summary>
    /// Tickets currently owned by this user.
    /// </summary>
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    /// <summary>
    /// Temporary seat reservations held by this user.
    /// </summary>
    public virtual ICollection<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();

    /// <summary>
    /// Waiting list entries for sold-out shows.
    /// </summary>
    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();

    /// <summary>
    /// Notification preferences for this user.
    /// </summary>
    public virtual ICollection<UserNotificationPref> NotificationPrefs { get; set; } = new List<UserNotificationPref>();

    /// <summary>
    /// Extended organizer information (if this user is an organizer).
    /// </summary>
    public virtual Organizer? Organizer { get; set; }

    /// <summary>
    /// Add-ons created by this user (if the user is an organizer).
    /// </summary>
    public virtual ICollection<AddOn> AddOns { get; set; } = new List<AddOn>();
}



