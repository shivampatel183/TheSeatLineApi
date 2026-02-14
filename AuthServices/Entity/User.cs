using System.ComponentModel.DataAnnotations;
using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.AuthServices.Entity
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserRole RoleId { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}
