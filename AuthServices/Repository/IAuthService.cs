using TheSeatLineApi.AuthServices.DTOs;

namespace TheSeatLineApi.AuthServices.Repository
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    }
}
