using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class ActiviteDto
    {
        public string CodeActivite { get; set; }
        public string LibelleActivite { get; set; }
        public int? IndustryId { get; set; }
    }
}