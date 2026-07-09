using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class CreateRegimeDto
    {
        public string Label { get; set; }
        public string DescriptionRegime { get; set; }
        public string Acronym { get; set; }
        public bool IsActive { get; set; }

        // Le FileType courant depuis lequel on crée le Regime.
        // Si IsActive = true, on lie automatiquement à ce FileType.
        public string CodeTypeDossier { get; set; }
    }
}