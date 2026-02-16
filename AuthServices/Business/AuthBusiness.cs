using TheSeatLineApi.AuthServices.DTOs;
using TheSeatLineApi.AuthServices.Helpers;
using TheSeatLineApi.AuthServices.Repository;
using TheSeatLineApi;
using TheSeatLineApi.AuthServices.Entity;
using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using Google.Apis.Auth;
using System.Data;
using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.AuthServices.Business
{
    public class AuthBusiness : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtTokenGenerator _jwt;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthBusiness(IUserRepository userRepo, JwtTokenGenerator jwt, AppDbContext context, IConfiguration config)
        {
            _userRepo = userRepo;
            _jwt = jwt;
            _context = context;
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
            };
            var refreshToken = _jwt.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            user.Id = await _userRepo.AddAsync(user);

            return new AuthResponseDto(
                _jwt.GenerateToken(user.Id, user.Email, user.FullName, user.RoleId),
                refreshToken,
                user.Email,
                user.FullName
            );
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email)
                ?? throw new Exception("Invalid credentials");

            if (string.IsNullOrEmpty(user.PasswordHash))
                throw new Exception("Please login with Google");

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");


            var newRefreshToken = _jwt.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new AuthResponseDto(
                _jwt.GenerateToken(user.Id, user.Email, user.FullName, user.RoleId),
                newRefreshToken,
                user.Email,
                user.FullName
            );
        }

        public async Task<AuthResponseDto> LoginWithGoogleAsync(string googleIdToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
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
                };
                user.Id = await _userRepo.AddAsync(user);
            }

            var accessToken = _jwt.GenerateToken(user.Id, user.Email, user.FullName, user.RoleId);
            var refreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.Users.Update(user); 
            await _context.SaveChangesAsync();

            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FullName);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var principal = _jwt.GetPrincipalFromExpiredToken(dto.AccessToken);
            var email = principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (email == null) throw new Exception("Invalid Token");

            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Invalid Refresh Token");
            }

            var newAccessToken = _jwt.GenerateToken(user.Id, user.Email, user.FullName, user.RoleId);
            var newRefreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto(newAccessToken, newRefreshToken, user.Email, user.FullName);
        }
    }
}
