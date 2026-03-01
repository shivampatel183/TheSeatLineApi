using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.AuthServices.DTOs
{
    public record UpdateProfileDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; init; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; init; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; init; }

        public DateTime? DateOfBirth { get; init; }

        [MaxLength(1024)]
        public string? AvatarUrl { get; init; }
    }
}
