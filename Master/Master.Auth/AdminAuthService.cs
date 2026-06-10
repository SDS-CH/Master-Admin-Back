#nullable disable
using Master.DIContainerCore;
using Master.DTO.AuthDTO;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Master.Auth
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly IDmsUserRepository<MasterAdminUsers> _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ERPMasterContext _dbContext;

        public AdminAuthService(
            IDmsUserRepository<MasterAdminUsers> userRepository,
            IOptions<JwtSettings> jwtSettings,
            ERPMasterContext dbContext)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _dbContext = dbContext;
        }

        public async Task<AuthResult> LoginAdmin(LoginAdminDTO dto)
        {
            try
            {
                MasterAdminUsers admin = null;

                if (!string.IsNullOrEmpty(dto.Email))
                    admin = await _userRepository.GetByEmail(dto.Email);

                if (admin == null)
                    return AuthResult.Fail("Invalid credentials.");

                if (!VerifyPassword(dto.Password, admin.Password))
                    return AuthResult.Fail("Invalid credentials.");

                if (admin.IsBlocked == true)
                    return AuthResult.Fail("Account is blocked.");

                if (!admin.IsActive)
                    return AuthResult.Fail("Account is inactive.");

                var accessToken = GenerateAccessToken(admin);
                var refreshTokenId = Guid.NewGuid().ToString("N");
                var refreshToken = GenerateRefreshToken(admin.Id, refreshTokenId);

                // Store/update refresh token
                var existing = await _dbContext.Set<AuthRefreshTokens>().FindAsync(admin.Id);
                if (existing != null)
                {
                    existing.ActiveRefreshTokenId = refreshTokenId;
                    existing.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationMinutes);
                    existing.IsCompromised = false;
                    existing.UpdatedAtUtc = DateTime.UtcNow;
                    _dbContext.Entry(existing).State = EntityState.Modified;
                }
                else
                {
                    // AdminRefreshTokens is separate from ErpUser tokens; store in master_admin_refresh_tokens if exists
                    // Since AuthRefreshTokens references ErpUsers, we skip DB storage for admin and rely on JWT validation
                }

                await _dbContext.SaveChangesAsync();

                return AuthResult.Ok(accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                return AuthResult.Fail(ex.Message);
            }
        }

        public async Task<AuthResult> RefreshToken(string refreshToken)
        {
            try
            {
                var principal = ValidateToken(refreshToken);
                if (principal == null)
                    return AuthResult.Fail("Invalid refresh token.");

                var userIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out var userId))
                    return AuthResult.Fail("Invalid token claims.");

                var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);

                var admin = await _dbContext.MasterAdminUsers.FindAsync(userId);
                if (admin == null || admin.IsBlocked == true || !admin.IsActive)
                    return AuthResult.Fail("User not found or inactive.");

                var newAccessToken = GenerateAccessToken(admin);
                var newRefreshTokenId = Guid.NewGuid().ToString("N");
                var newRefreshToken = GenerateRefreshToken(admin.Id, newRefreshTokenId);

                return AuthResult.Ok(newAccessToken, newRefreshToken);
            }
            catch (Exception ex)
            {
                return AuthResult.Fail(ex.Message);
            }
        }

        public async Task<bool> ChangePassword(int userId, ChangePasswordDTO dto)
        {
            try
            {
                var admin = await _dbContext.MasterAdminUsers.FindAsync(userId);
                if (admin == null) return false;

                if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, admin.Password))
                    return false;

                admin.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                admin.LastPasswordChangeDate = DateTime.UtcNow;
                admin.EditTime = DateTime.UtcNow;

                _dbContext.Entry(admin).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;

            // Normalize PHP-style $2y$ prefix to $2a$ which BCrypt.Net-Next accepts
            var hash = storedHash.StartsWith("$2y$")
                ? "$2a$" + storedHash[4..]
                : storedHash;

            try
            {
                return BCrypt.Net.BCrypt.Verify(inputPassword, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Hash is not BCrypt at all (plain text, MD5, SHA...) — cannot verify safely
                return false;
            }
        }

        private string GenerateAccessToken(MasterAdminUsers admin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, admin.Email ?? string.Empty),
                new Claim(ClaimTypes.Email, admin.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(int userId, string jti)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
                var handler = new JwtSecurityTokenHandler();
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = key
                };
                return handler.ValidateToken(token, parameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}
