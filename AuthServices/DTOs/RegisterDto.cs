namespace TheSeatLineApi.AuthServices.DTOs
{
    public record RegisterDto(
        string FullName,
        string Email,
        string Password
    );

}
