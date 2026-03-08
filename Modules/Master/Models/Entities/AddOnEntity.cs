using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Modules.BookingModule.Models.Entities;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("AddOns")]
    public class AddOn
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Optional: if add-ons are owned by a specific partner (organizer).
        /// </summary>
        public Guid? OrganizerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "char(3)")]
        public string Currency { get; set; } = "INR";

        /// <summary>
        /// Maximum quantity of this add-on that can be added per booking.
        /// </summary>
        public int? MaxPerBooking { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(OrganizerId))]
        public User? Organizer { get; set; }
        public ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();  
    }
}



