using System;

namespace Master.DTO.DTOs
{
    public class ErpUserTenantDTO
    {
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public Guid TenantGuid { get; set; }
        public string EntityName { get; set; }
        public DateTime AddNewTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string CryptedCs { get; set; }
        public bool IsErpUser { get; set; }
    }
}
