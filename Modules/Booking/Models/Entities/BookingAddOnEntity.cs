using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Modules.MasterModule.Models.Entities;

namespace TheSeatLineApi.Modules.BookingModule.Models.Entities
{
    [Table("BookingAddOns")]
    public class BookingAddOn
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BookingId { get; set; }

        [Required]
        public Guid AddOnId { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// Price of the add-on at the time of booking (historical).
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Navigation properties
        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; } = null!;

        [ForeignKey(nameof(AddOnId))]
        public virtual AddOn AddOn { get; set; } = null!;
    }
}



