using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.DTOs
{
    public class TicketCategoryInsertDTO
    {
        [Required]
        public Guid EventShowId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int TotalQuantity { get; set; }
    }

    public class TicketCategorySelectDTO
    {
        public Guid Id { get; set; }
        public Guid EventShowId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
        public int SoldQuantity { get; set; }
    }
}
