using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs

{
    public class ActivityDto
    {
        public string CodeActivite { get; set; } = string.Empty;
        public string LibelleActivite { get; set; } = string.Empty;
        public string ModuleOperation { get; set; } = string.Empty;
        public string? ConcernFacturation { get; set; }
        public int Session { get; set; }
        public DateTime AddNewTime { get; set; }
        public DateTime EditTime { get; set; }
        public Guid TenantId { get; set; }
        public int? IndustryId { get; set; }
    }
}
