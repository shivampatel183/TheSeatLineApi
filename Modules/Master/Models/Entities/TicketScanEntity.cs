using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("TicketScans")]
    public class TicketScan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        public DateTime ScannedAt { get; set; } = DateTime.UtcNow;

        public Guid? ScannedBy { get; set; } // User who scanned (venue staff)

        // Navigation
        [ForeignKey(nameof(TicketId))]
        public virtual Ticket Ticket { get; set; } = null!;
    }
}



