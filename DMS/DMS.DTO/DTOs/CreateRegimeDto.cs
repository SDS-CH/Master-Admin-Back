using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class CreateRegimeDto
    {
        public string CodeRegime { get; set; } = string.Empty;
        public string? Label { get; set; }
        public string? DescriptionRegime { get; set; }
        public string? Acronym { get; set; }
        public string FileTypeCode { get; set; } = string.Empty;
    }
}