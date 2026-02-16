namespace TheSeatLineApi.MasterServices.DTOs
{
    public class EventDTOs
    {
        public class EventInsertDTO
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public string? Description { get; set; }

            public string? Language { get; set; }

            public int DurationMinutes { get; set; }

            public string? PosterUrl { get; set; }

            public string? TrailerUrl { get; set; }

            public DateTime? ReleaseDate { get; set; }

            public bool IsActive { get; set; }
        }
        public class EventSelectDTO
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public string? Description { get; set; }

            public string? Language { get; set; }

            public int DurationMinutes { get; set; }

            public string? PosterUrl { get; set; }

            public string? TrailerUrl { get; set; }

            public DateTime? ReleaseDate { get; set; }

            // Location-related fields
            public List<int>? CityIds { get; set; }
            public List<string>? CityNames { get; set; }
            public List<string>? VenueNames { get; set; }
            public double? DistanceKm { get; set; }
            public List<DateTime>? UpcomingShowDates { get; set; }
        }
    }
}
