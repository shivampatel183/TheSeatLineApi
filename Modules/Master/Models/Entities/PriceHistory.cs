using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("PriceHistory")]
    public class PriceHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TicketCategoryId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal OldPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal NewPrice { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ID of the user who changed the price (optional).
        /// </summary>
        public Guid? ChangedBy { get; set; }

        // Navigation
        [ForeignKey(nameof(TicketCategoryId))]
        public virtual TicketCategory TicketCategory { get; set; } = null!;
    }
}



