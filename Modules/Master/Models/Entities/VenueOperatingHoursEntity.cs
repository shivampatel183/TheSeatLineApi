using TheSeatLineApi.Data.Entities;

public class VenueOperatingHours
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VenueId { get; set; }
    public byte DayOfWeek { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public bool IsClosed { get; set; }

    public Venue Venue { get; set; } = null!;
}



