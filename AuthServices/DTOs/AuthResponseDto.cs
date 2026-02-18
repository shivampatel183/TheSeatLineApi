using TheSeatLineApi.AuthServices.DTOs;

public record AuthResponseDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiresAt { get; init; }
    public string TokenType { get; init; } = "Bearer";
    public UserProfileDto User { get; init; }
}