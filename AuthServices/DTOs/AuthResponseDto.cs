namespace TheSeatLineApi.AuthServices.DTOs
{
    public record AuthResponseDto(
        string Token,
        string RefreshToken,
        string Email,
        string FullName
    );

}
