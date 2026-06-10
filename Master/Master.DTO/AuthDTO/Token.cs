namespace Master.DTO.AuthDTO
{
    public class Token
    {
        public double Expires_in;
        public string Access_token;

        [System.Text.Json.Serialization.JsonIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public string RefreshToken;
        public string CryptedCs;
    }
}
