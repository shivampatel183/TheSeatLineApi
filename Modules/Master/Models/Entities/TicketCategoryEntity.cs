using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Modules.MasterModule.Models.Entities;


[Table("TicketCategories")]
public class TicketCategory
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid EventShowId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int TotalQuantity { get; set; }

    public int SoldQuantity { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Concurrency token for optimistic locking.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(EventShowId))]
    public virtual EventShow EventShow { get; set; } = null!;

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
}


