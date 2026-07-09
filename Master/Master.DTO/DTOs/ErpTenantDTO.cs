using System;

namespace Master.DTO.DTOs
{
    public class ErpTenantDTO
    {
        public int Id { get; set; }
        public Guid? TenantId { get; set; }

        public string? EntityName { get; set; }

        public DateTime? AddNewTime { get; set; }
        public int? DbInstanceId { get; set; }
        public string? DbInstanceName { get; set; }
    }
}
