using System;

namespace Master.DTO.DTOs
{
    public class DbInstanceDTO
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public string AdminUser { get; set; }
        public string AdminPassword { get; set; }
        public string DbUser { get; set; }
        public string Password { get; set; }
        public string ReadOnlyUser { get; set; }
        public string ReadOnlyPassword { get; set; }
        public string DbName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
        public DateTime AddNewTime { get; set; }
        public string EdmTempDirectory { get; set; }
    }
}
