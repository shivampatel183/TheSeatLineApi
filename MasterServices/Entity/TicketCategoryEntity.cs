using System;

public class TicketCategory : BaseEntity
{
    public Guid EventShowId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int TotalQuantity { get; set; }
    public int SoldQuantity { get; set; }

    public EventShow EventShow { get; set; } = null!;
}
