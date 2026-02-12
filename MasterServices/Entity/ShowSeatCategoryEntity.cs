using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSeatLineApi.Entity
{
    public class ShowSeatCategoryEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShowId { get; set; }

        [Required]
        public string SeatCategoryName { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("ShowId")]
        public ShowEntity Show { get; set; }
    }
}
