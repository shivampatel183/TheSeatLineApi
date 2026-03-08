using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("SeatCoordinates")]
    public class SeatCoordinate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SeatId { get; set; }

        /// <summary>
        /// GeoJSON or SVG path describing the seat shape.
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string? Shape { get; set; }

        /// <summary>
        /// X coordinate (pixel-based for grid layouts).
        /// </summary>
        public int? X { get; set; }

        /// <summary>
        /// Y coordinate (pixel-based).
        /// </summary>
        public int? Y { get; set; }

        /// <summary>
        /// Width of the seat representation.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Height of the seat representation.
        /// </summary>
        public int? Height { get; set; }

        // Navigation
        [ForeignKey(nameof(SeatId))]
        public virtual Seat Seat { get; set; } = null!;
    }
}



