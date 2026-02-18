using TheSeatLineApi.AuthServices.DTOs;

namespace TheSeatLineApi.AuthServices.Repository
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    }
}
