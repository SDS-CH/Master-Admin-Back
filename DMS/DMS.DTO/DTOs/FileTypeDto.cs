using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class FileTypeDto
    {
        public string? CodeTypeDossier { get; set; }
        public string LibelleTypeDossier { get; set; }
        public string Activite { get; set; }
        public string? ActiviteLibelle { get; set; }
        public string? SensTrafic { get; set; }
        public string? ModeTransport { get; set; }
        public bool Mensuel { get; set; }
        public string? PlanOperationOuverture { get; set; }
        public bool Disponible { get; set; }
        public string? CustomValueLabel { get; set; }
    }
}