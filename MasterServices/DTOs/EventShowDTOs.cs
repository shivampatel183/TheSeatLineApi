using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.DTOs
{
    public class EventShowInsertDTO
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public byte Status { get; set; }

        public int MaxCapacity { get; set; }
    }

    public class EventShowSelectDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventTitle { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public byte Status { get; set; }
        public int MaxCapacity { get; set; }
    }
}
