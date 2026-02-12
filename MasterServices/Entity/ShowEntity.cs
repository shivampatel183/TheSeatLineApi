using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheSeatLineApi.MasterServices.Entity;

namespace TheSeatLineApi.Entity
{
    public class ShowEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public DateTime ShowTime { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("EventId")]
        public EventEntity Event { get; set; }

        [ForeignKey("VenueId")]
        public VenueEntity Venue { get; set; }
    }
}
