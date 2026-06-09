#nullable disable
using Master.Auth;
using Master.DTO.AuthDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Master.API.Controllers.Authentication
{
    [Route("")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string RefreshTokenCookieName = "master_refresh_token";
        private const string RefreshCsrfCookieName = "master_refresh_csrf";
        private const string RefreshCsrfHeaderName = "X-Refresh-CSRF";

        private readonly IAdminAuthService _authService;
        private readonly IWebHostEnvironment _env;

        public AuthController(IAdminAuthService authService, IWebHostEnvironment env)
        {
            _authService = authService;
            _env = env;
        }

        [HttpPost("/login-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginAdminDTO dto)
        {
            var result = await _authService.LoginAdmin(dto);
            if (!result.Succeeded)
                return Unauthorized(new { message = result.Message ?? "Invalid credentials." });

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(new { accessToken = result.AccessToken });
        }

        [HttpPost("/auth/sign-out")]
        [AllowAnonymous]
        public new IActionResult SignOut()
        {
            ClearRefreshCookie();
            return Ok();
        }

        [HttpPost("/auth/refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieName];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { message = "No refresh token." });

            var result = await _authService.RefreshToken(refreshToken);
            if (!result.Succeeded)
            {
                ClearRefreshCookie();
                return Unauthorized(new { message = result.Message ?? "Invalid refresh token." });
            }

            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(new { accessToken = result.AccessToken });
        }

        [HttpPost("/auth/reset-pass")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var idStr = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idStr, out var userId))
                return BadRequest(new { message = "Invalid user." });

            var success = await _authService.ChangePassword(userId, dto);
            if (!success)
                return BadRequest(new { message = "Password change failed. Check your old password." });

            return Ok(new { message = "Password changed successfully." });
        }

        private void SetRefreshTokenCookie(string token)
        {
            // SameSite=None + Secure=true allows the cookie to be sent cross-origin (Angular :4200 → API :7163)
            // Browsers require Secure=true when SameSite=None; localhost gets a special exemption in Chrome/Firefox
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                Path = "/auth"
            };
            Response.Cookies.Append(RefreshTokenCookieName, token, options);
        }

        private void ClearRefreshCookie()
        {
            Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions { Path = "/auth" });
        }
    }
}
