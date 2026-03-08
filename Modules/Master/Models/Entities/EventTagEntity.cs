public class EventTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public string Tag { get; set; } = null!;

    public Event Event { get; set; } = null!;
}



