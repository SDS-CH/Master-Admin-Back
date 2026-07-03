using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class RegimeDto
    {
        public string CodeRegime { get; set; }
        public string Label { get; set; }
        public string DescriptionRegime { get; set; }
        public string Acronym { get; set; }
        public bool IsActive { get; set; }
    }
}
