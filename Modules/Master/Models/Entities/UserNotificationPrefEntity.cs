using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("UserNotificationPrefs")]
    public class UserNotificationPref
    {
        [Key, Column(Order = 0)]
        public Guid UserId { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public string NotificationType { get; set; } = null!;

        /// <summary>
        /// Indicates whether the user wants to receive this type of notification.
        /// </summary>
        public bool Enabled { get; set; } = true;

        // Navigation
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}



