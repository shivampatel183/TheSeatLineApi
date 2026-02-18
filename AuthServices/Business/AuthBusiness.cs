using Google.Apis.Auth;
using TheSeatLineApi.AuthServices.DTOs;
using TheSeatLineApi.AuthServices.Entity;
using TheSeatLineApi.AuthServices.Helpers;
using TheSeatLineApi.AuthServices.Repository;
using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.AuthServices.Business
{
    public class AuthBusiness : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtTokenGenerator _jwt;
        private readonly IConfiguration _config;

        public AuthBusiness(IUserRepository userRepo, JwtTokenGenerator jwt, IConfiguration config)
        {
            _userRepo = userRepo;
            _jwt = jwt;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("User already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                IsEmailVerified = false,
                RoleId = UserRole.User,
                UserStatus = UserStatus.Active
            };

            user.RefreshToken = _jwt.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            user.Id = await _userRepo.AddAsync(user);

            return BuildAuthResponse(user, user.RefreshToken);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email)
                ?? throw new Exception("Invalid credentials");

            if (string.IsNullOrEmpty(user.PasswordHash))
                throw new Exception("Please login with Google");

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            user.RefreshToken = _jwt.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return BuildAuthResponse(user, user.RefreshToken);
        }

        public async Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _config["Google:client_id"]! }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(googleIdToken, settings);
            }
            catch
            {
                throw new Exception("Invalid Google Token");
            }

            var user = await _userRepo.GetByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    FullName = payload.Name,
                    Email = payload.Email,
                    IsEmailVerified = payload.EmailVerified,
                    PasswordHash = null,
                    RoleId = UserRole.User,
                    UserStatus = UserStatus.Active
                };

                user.Id = await _userRepo.AddAsync(user);
            }

            user.RefreshToken = _jwt.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return BuildAuthResponse(user, user.RefreshToken);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var principal = _jwt.GetPrincipalFromExpiredToken(dto.AccessToken);
            var email = principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (email == null)
                throw new Exception("Invalid Token");

            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Invalid Refresh Token");

            var newRefreshToken = _jwt.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);

            return new AuthResponseDto(
                _jwt.GenerateToken(user.Id, user.Email, user.FullName, user.RoleId),
                newRefreshToken,
                user.Email,
                user.FullName
            );
        }

        private AuthResponseDto BuildAuthResponse(User user, string? refreshToken)
        {
            return new AuthResponseDto(
                _jwt.GenerateToken(user.Id, user.Email, user.FullName, user.RoleId),
                refreshToken ?? string.Empty,
                user.Email,
                user.FullName
            );
        }
    }
}
