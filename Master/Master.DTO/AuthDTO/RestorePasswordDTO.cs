namespace Master.DTO.AuthDTO
{
    public class RestorePasswordDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public int ClientID { get; set; }
    }
}
