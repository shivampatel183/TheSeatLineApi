using TheSeatLineApi.Data.Entities;

public class City
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Slug { get; set; }

    public ICollection<Venue> Venues { get; set; } = new List<Venue>();
}


