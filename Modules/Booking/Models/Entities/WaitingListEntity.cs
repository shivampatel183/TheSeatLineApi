using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Modules.MasterModule.Models.Entities;

namespace TheSeatLineApi.Modules.BookingModule.Models.Entities
{
    [Table("WaitingList")]
    public class WaitingList
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EventShowId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the user has already been notified of availability.
        /// </summary>
        public bool Notified { get; set; }

        /// <summary>
        /// When the user was last notified (if ever).
        /// </summary>
        public DateTime? NotifiedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(EventShowId))]
        public virtual EventShow EventShow { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}



