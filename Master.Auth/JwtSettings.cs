#nullable disable
namespace Master.Auth
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; } = 60;
        public int RefreshTokenExpirationMinutes { get; set; } = 1440;
    }
}
