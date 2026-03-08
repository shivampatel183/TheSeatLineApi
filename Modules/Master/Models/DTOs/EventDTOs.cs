using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.DTOs
{
    public class EventInsertDTO
    {
        [Required, MaxLength(300)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public Guid VenueId { get; set; }

        public Guid? CategoryId { get; set; }          // New

        [Required]
        public byte EventType { get; set; }

        public List<string> Tags { get; set; } = new();   // Changed from string

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime? EndDateTime { get; set; }

        [Required]
        public string Timezone { get; set; } = null!;

        public string? Language { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        public string? BannerImageUrl { get; set; }
        public string? Performers { get; set; }        // Still JSON string
    }

    public class EventSelectDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public byte EventType { get; set; }
        public string? Language { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string? BannerImageUrl { get; set; }
        public byte Status { get; set; }

        // Venue & City
        public Guid VenueId { get; set; }
        public string VenueName { get; set; } = null!;
        public string City { get; set; } = null!;          // City name
        public string CitySlug { get; set; } = null!;      // For filtering
        public string State { get; set; } = null!;

        // Category
        public string? CategoryName { get; set; }          // New

        // Tags (optional – may be omitted in list view)
        public List<string>? Tags { get; set; }
    }

    public class EventDetailDTO : EventSelectDTO
    {
        public string? Performers { get; set; }            // JSON string
        public byte? AgeRestriction { get; set; }
        public string Timezone { get; set; } = null!;
        public int MaxCapacity { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrenceRule { get; set; }

        // Additional collections
        public List<EventImageDto> Images { get; set; } = new();
        public List<EventShowDto> Shows { get; set; } = new();
    }

    public class EventImageDto
    {
        public string ImageUrl { get; set; } = null!;
        public int SortOrder { get; set; }
    }

    public class EventShowDto
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public byte Status { get; set; }
        public int AvailableSeats { get; set; }   // We'll compute this later
    }


}


