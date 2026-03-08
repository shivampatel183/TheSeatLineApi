using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("NotificationTemplates")]
    public class NotificationTemplate
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Optional: if the template belongs to a specific organizer (partner).
        /// If null, it's a system-wide default template.
        /// </summary>
        public Guid? OrganizerId { get; set; }

        /// <summary>
        /// Type of notification (e.g., "BookingConfirmed", "EventReminder", "PaymentReceived").
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string EventType { get; set; } = null!;

        /// <summary>
        /// Delivery channel: 1 = Email, 2 = SMS, 3 = Push.
        /// </summary>
        public byte Channel { get; set; }

        /// <summary>
        /// Subject line (used for email; optional for other channels).
        /// </summary>
        [MaxLength(255)]
        public string? Subject { get; set; }

        /// <summary>
        /// Body content of the notification. May contain placeholders like {{EventName}}.
        /// </summary>
        [Required]
        public string Body { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        // Navigation
        [ForeignKey(nameof(OrganizerId))]
        public virtual User? Organizer { get; set; }
    }
}



