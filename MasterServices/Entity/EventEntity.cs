using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.Entity
{
    public class EventEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Language { get; set; }

        public int DurationMinutes { get; set; }

        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
