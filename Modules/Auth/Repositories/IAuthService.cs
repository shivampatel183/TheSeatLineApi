using TheSeatLineApi.Modules.AuthModule.Models.DTOs;

namespace TheSeatLineApi.Modules.AuthModule.Repositories
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    }
}



