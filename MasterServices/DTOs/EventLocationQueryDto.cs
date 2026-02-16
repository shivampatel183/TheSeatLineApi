using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.DTOs
{
    public class EventLocationQueryDto
    {
        /// <summary>
        /// Filter by specific city ID
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// User's current latitude (for GPS-based search)
        /// </summary>
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// User's current longitude (for GPS-based search)
        /// </summary>
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Search radius in kilometers (default: 25km)
        /// </summary>
        [Range(1, 500, ErrorMessage = "Radius must be between 1 and 500 km")]
        public int RadiusKm { get; set; } = 25;

        /// <summary>
        /// Show events from this date onwards
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Show events until this date
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Page number for pagination (default: 1)
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page (default: 20)
        /// </summary>
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 20;
    }
}
