using System;

namespace Master.DTO.DTOs
{
    public class MasterErpmoduleClientDTO
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int TenantId { get; set; }
        public int MaxAgencies { get; set; }
        public string TenantName { get; set; }
        public string ModuleName { get; set; }
    }
}
