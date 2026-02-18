using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.MasterServices.DTOs
{
    public class VenueSelectDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string? Address { get; set; }
    }
    public record CreateVenueRequestDto
    {
        [Required, MaxLength(200)]
        public string Name { get; init; } = null!;

        [Required]
        public byte VenueType { get; init; }

        [Required]
        public string AddressLine1 { get; init; } = null!;

        [Required]
        public string City { get; init; } = null!;

        [Required]
        public string State { get; init; } = null!;

        [Required]
        public string Country { get; init; } = null!;

        [Required]
        public string PostalCode { get; init; } = null!;

        public string? Description { get; init; }
    }

    public record VenueSummaryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string City { get; init; } = null!;
        public string State { get; init; } = null!;
    }

}
