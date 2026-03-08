using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.DTOs
{
    public class SeatInsertDTO
    {
        [Required]
        public Guid EventId { get; set; }

        public string? Section { get; set; }
        public string? Row { get; set; }

        [Required, MaxLength(10)]
        public string SeatNumber { get; set; } = null!;

        [Required]
        public byte SeatType { get; set; }

        [Required]
        public decimal BasePrice { get; set; }

        public bool IsAisle { get; set; }
        public bool IsWindow { get; set; }
        public bool IsEmergencyExit { get; set; }
    }

    public class SeatBulkInsertDTO
    {
        [Required]
        public Guid EventId { get; set; }

        public string? Section { get; set; }

        [Required]
        public byte SeatType { get; set; }

        [Required]
        public decimal BasePrice { get; set; }

        [Required, MaxLength(10)]
        public string RowLabel { get; set; } = null!;

        [Required, Range(1, 100)]
        public int StartNumber { get; set; }

        [Required, Range(1, 100)]
        public int EndNumber { get; set; }
    }

    public class SeatSelectDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string? Section { get; set; }
        public string? Row { get; set; }
        public string SeatNumber { get; set; } = null!;
        public byte SeatType { get; set; }
        public byte Status { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; } = null!;
        public bool IsAisle { get; set; }
        public bool IsWindow { get; set; }
        public bool IsEmergencyExit { get; set; }
    }
}



