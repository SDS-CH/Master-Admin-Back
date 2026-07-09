using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Master.DTO.DTOs
{
    public class ErpUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool ResetPasswordIsNeeded { get; set; }
        public string Photo { get; set; }
        public DateTime AddNewTime { get; set; }
        public DateTime? EditTime { get; set; }
        public string PasswordSalt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool? IsBlocked { get; set; }
        public int? NumberOfAttemptsLogin { get; set; }
        public bool? HasM2f { get; set; }
        public DateTime? M2fExpireDateTime { get; set; }
        public string AuthCode { get; set; }
        public int? AuthCodeAttempts { get; set; }
        public DateTime? LastAttempts { get; set; }
        public int? NumberOfCodeGenerated { get; set; }
        public DateTime? LastCodeGeneratedTime { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public bool IsErpUser { get; set; }
        public List<ErpUserTenantDTO> Tenants { get; set; } = new List<ErpUserTenantDTO>();
    }
}
