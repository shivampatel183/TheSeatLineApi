
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("TicketTransfers")]
    public class TicketTransfer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid FromUserId { get; set; }

        [Required]
        public Guid ToUserId { get; set; }

        public DateTime TransferredAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(TicketId))]
        public virtual Ticket Ticket { get; set; } = null!;

        [ForeignKey(nameof(FromUserId))]
        public virtual User FromUser { get; set; } = null!;

        [ForeignKey(nameof(ToUserId))]
        public virtual User ToUser { get; set; } = null!;
    }
}



