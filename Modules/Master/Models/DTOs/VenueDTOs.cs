
    public class VenueListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string CityName { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class VenueDetailDto : VenueListDto
    {
        public string? AddressLine2 { get; set; }
        public string PostalCode { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Timezone { get; set; } = null!;
        public int TotalCapacity { get; set; }
        public List<string>? Amenities { get; set; }
        public List<string>? AccessibilityFeatures { get; set; }
        public List<string>? MediaGallery { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<OperatingHourDto> OperatingHours { get; set; } = new();
    }

    public class OperatingHourDto
    {
        public byte DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }

    public class VenueCreateDto
    {
        public string Name { get; set; } = null!;
        public Guid CityId { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string PostalCode { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Timezone { get; set; } = null!;
        public int TotalCapacity { get; set; }
        public List<string>? Amenities { get; set; }
        public List<string>? AccessibilityFeatures { get; set; }
        public List<string>? MediaGallery { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<OperatingHourDto> OperatingHours { get; set; } = new();
    }

    public class VenueUpdateDto : VenueCreateDto { }
