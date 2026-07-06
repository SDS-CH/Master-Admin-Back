using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class TranslationDto
    {
        public string SourceLabel { get; set; }
        public int? IdFr { get; set; }
        public string TranslatedLabelFr { get; set; }
        public int? IdDe { get; set; }
        public string TranslatedLabelDe { get; set; }
    }
}
