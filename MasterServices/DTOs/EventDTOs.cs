using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.DTOs
{
    public class EventInsertDTO
    {
        [Required, MaxLength(300)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public Guid VenueId { get; set; }

        [Required]
        public byte EventType { get; set; }

        public string? Tags { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public string Timezone { get; set; } = null!;

        public string? Language { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        public string? BannerImageUrl { get; set; }
        public string? Performers { get; set; }
    }

    public class EventSelectDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public byte EventType { get; set; }
        public string? Language { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? BannerImageUrl { get; set; }
        public byte Status { get; set; }

        // Venue info
        public Guid VenueId { get; set; }
        public string VenueName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
    }
}
