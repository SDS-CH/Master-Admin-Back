namespace Master.DTO.AuthDTO
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool? HasM2F { get; set; }
    }

    public class AdminChangePasswordDTO : ChangePasswordDTO
    {
        public int UserID { get; set; }
    }
}
