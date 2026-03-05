public class EventImage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int SortOrder { get; set; }

    public Event Event { get; set; } = null!;
}
