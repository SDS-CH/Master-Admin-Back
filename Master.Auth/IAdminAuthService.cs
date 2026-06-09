#nullable disable
using Master.DTO.AuthDTO;
using System.Threading.Tasks;

namespace Master.Auth
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }

        public static AuthResult Ok(string access, string refresh) => new AuthResult
        {
            Succeeded = true,
            AccessToken = access,
            RefreshToken = refresh
        };

        public static AuthResult Fail(string message = null) => new AuthResult
        {
            Succeeded = false,
            Message = message
        };
    }

    public interface IAdminAuthService
    {
        Task<AuthResult> LoginAdmin(LoginAdminDTO dto);
        Task<AuthResult> RefreshToken(string refreshToken);
        Task<bool> ChangePassword(int userId, ChangePasswordDTO dto);
    }
}
