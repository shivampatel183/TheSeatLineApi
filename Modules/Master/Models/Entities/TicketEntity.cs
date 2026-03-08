using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BookingId { get; set; }

        [Required]
        public Guid EventShowId { get; set; }

        public Guid? SeatId { get; set; }

        public Guid? TicketCategoryId { get; set; }

        [Required]
        public Guid OwnerUserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TicketNumber { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string QRCode { get; set; } = null!;

        public byte Status { get; set; } // 0=Active,1=Used,2=Transferred,3=Refunded

        public Guid? TransferredToUserId { get; set; }

        public DateTime? UsedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; } = null!;

        [ForeignKey(nameof(EventShowId))]
        public virtual EventShow EventShow { get; set; } = null!;

        [ForeignKey(nameof(SeatId))]
        public virtual Seat? Seat { get; set; }

        [ForeignKey(nameof(TicketCategoryId))]
        public virtual TicketCategory? TicketCategory { get; set; }

        [ForeignKey(nameof(OwnerUserId))]
        public virtual User OwnerUser { get; set; } = null!;

        [ForeignKey(nameof(TransferredToUserId))]
        public virtual User? TransferredToUser { get; set; }

        // Collections
        public virtual ICollection<TicketScan> Scans { get; set; } = new List<TicketScan>();
        public virtual ICollection<TicketTransfer> Transfers { get; set; } = new List<TicketTransfer>();
    }
}



