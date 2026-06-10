namespace Master.DTO.Users
{
    public class RequestPasswordDTO
    {
        public string Email { get; set; }
        public string CaptchaToken { get; set; }
        public int ClientID { get; set; }
        public string CryptedCs { get; set; }
    }
}
