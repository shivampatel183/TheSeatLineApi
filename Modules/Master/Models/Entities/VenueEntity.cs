using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSeatLineApi.Data.Entities
{
    [Table("Venues")]
    public class Venue
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CityId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public byte VenueType { get; set; }  // 1=Theatre,2=Stadium,3=ConcertHall, etc.

        public string? Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string AddressLine1 { get; set; } = null!;

        [MaxLength(255)]
        public string? AddressLine2 { get; set; }

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; } = null!;

        [Column(TypeName = "decimal(10,8)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal? Longitude { get; set; }

        [Required]
        [MaxLength(60)]
        public string Timezone { get; set; } = null!;

        public int TotalCapacity { get; set; }

        /// <summary>JSON array of amenities.</summary>
        [Column(TypeName = "nvarchar(max)")]
        public string? Amenities { get; set; }

        /// <summary>JSON array of accessibility features.</summary>
        [Column(TypeName = "nvarchar(max)")]
        public string? AccessibilityFeatures { get; set; }

        /// <summary>JSON array of media gallery URLs.</summary>
        [Column(TypeName = "nvarchar(max)")]
        public string? MediaGallery { get; set; }

        [MaxLength(255)]
        public string? ContactEmail { get; set; }

        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(512)]
        public string? WebsiteUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; } = null!;

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<VenueOperatingHours> OperatingHours { get; set; } = new List<VenueOperatingHours>();
    }
}