using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.AuthModule.Models.Entities
{
    [Table("Organizers")]
    public class Organizer
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key to the associated user account.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Tax identification number (e.g., GST, VAT, EIN).
        /// </summary>
        [MaxLength(50)]
        public string? TaxId { get; set; }

        // Bank details for payouts
        [MaxLength(100)]
        public string? BankAccountNumber { get; set; }

        [MaxLength(100)]
        public string? BankIfscCode { get; set; }   // For India, or routing number for other countries

        [MaxLength(200)]
        public string? BankName { get; set; }

        [MaxLength(200)]
        public string? AccountHolderName { get; set; }

        [MaxLength(200)]
        public string? ContactEmail { get; set; }   // Separate from login email if needed

        [MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(500)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Admin verification flag – whether the organizer has been approved.
        /// </summary>
        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}



