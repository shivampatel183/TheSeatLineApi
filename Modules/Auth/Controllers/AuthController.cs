using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Modules.AuthModule.Models.DTOs;
using TheSeatLineApi.Modules.AuthModule.Helpers;
using TheSeatLineApi.Modules.AuthModule.Repositories;
using TheSeatLineApi.Shared;

namespace TheSeatLineApi.Modules.AuthModule.Controllerss
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepo;

        public AuthController(IAuthService authService, IUserRepository userRepo)
        {
            _authService = authService;
            _userRepo = userRepo;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<Response<UserProfileDto>> GetProfile()
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userRepo.GetByIdAsync(userId)
                    ?? throw new Exception("User not found");

                var profile = new UserProfileDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    AvatarUrl = user.AvatarUrl,
                    IsEmailVerified = user.IsEmailVerified,
                    IsPhoneVerified = user.IsPhoneVerified,
                    LastLoginAt = user.LastLoginAt,
                    CreatedAt = user.CreatedAt
                };

                return Response<UserProfileDto>.Ok(profile);
            }
            catch (Exception ex)
            {
                return Response<UserProfileDto>.Fail(ex.Message);
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<Response<UserProfileDto>> UpdateProfile(UpdateProfileDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var user = await _userRepo.GetByIdAsync(userId)
                    ?? throw new Exception("User not found");

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;
                user.DateOfBirth = dto.DateOfBirth;
                user.AvatarUrl = dto.AvatarUrl;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = userId;

                var updateduser = await _userRepo.UpdateAsync(user);
                var profile = new UserProfileDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    AvatarUrl = user.AvatarUrl,
                    IsEmailVerified = user.IsEmailVerified,
                    IsPhoneVerified = user.IsPhoneVerified,
                    LastLoginAt = user.LastLoginAt,
                    CreatedAt = user.CreatedAt
                };

                return Response<UserProfileDto>.Ok(profile);
            }
            catch (Exception ex)
            {
                return Response<UserProfileDto>.Fail(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<Response<AuthResponseDto>> Register(RegisterRequestDto dto)
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
        public async Task<Response<AuthResponseDto>> Login(LoginRequestDto dto)
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
        public async Task<Response<AuthResponseDto>> GoogleLogin(GoogleLoginDto dto)
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
        public async Task<Response<AuthResponseDto>> RefreshToken(RefreshTokenDto dto)
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



