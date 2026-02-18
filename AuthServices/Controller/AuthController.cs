using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.AuthServices.DTOs;
using TheSeatLineApi.AuthServices.Repository;
using TheSeatLineApi.Common;

namespace TheSeatLineApi.AuthServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<Response<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            try
            {
                return Response<AuthResponseDto>.Ok(await _authService.RegisterAsync(dto), "Registration success");
            }
            catch (Exception ex)
            {
                return Response<AuthResponseDto>.Fail(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<Response<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                return Response<AuthResponseDto>.Ok(await _authService.LoginAsync(dto));
            }
            catch (Exception ex)
            {
                return Response<AuthResponseDto>.Fail(ex.Message);
            }
        }

        [HttpPost("google-login")]
        public async Task<Response<AuthResponseDto>> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            try
            {
                return Response<AuthResponseDto>.Ok(await _authService.LoginWithGoogleAsync(dto.IdToken));
            }
            catch (Exception ex)
            {
                return Response<AuthResponseDto>.Fail(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<Response<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            try
            {
                return Response<AuthResponseDto>.Ok(await _authService.RefreshTokenAsync(dto));
            }
            catch (Exception ex)
            {
                return Response<AuthResponseDto>.Fail(ex.Message);
            }
        }
    }
}
