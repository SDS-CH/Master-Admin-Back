using System;
using System.Collections.Generic;

namespace DMS.DTO.DTOs
{
    public class MilestoneStepDto
    {
        public string CodeEtape { get; set; } = string.Empty;
        public string LibelleEtape { get; set; } = string.Empty;
        public string? CategorieEtape { get; set; }
        public int? OrdreEtape { get; set; }
        public bool? IsActive { get; set; }

        // Audit
        public DateTime AddNewTime { get; set; }
        public DateTime EditTime { get; set; }
    }

    public class FileTypeMilestoneDto : MilestoneStepDto
    {
        public int MappingId { get; set; }
        public string FileType { get; set; } = string.Empty;
        public bool? Obligatoire { get; set; }

        // Gestion du délai/avertissement, alignée sur le pattern GestionDuree/DelaiEtape
        public bool GestionDelai { get; set; }
        public int? LimiteAvertissement { get; set; }
    }

    public class AddFileTypeMilestonesDto
    {
        public List<string> StepCodes { get; set; } = new();
        public bool IsActive { get; set; }
    }

    public class CreateMilestoneStepDto
    {
        public string LibelleEtape { get; set; } = string.Empty;
        public string? CategorieEtape { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateFileTypeMilestoneDto
    {
        public bool? Obligatoire { get; set; }
        public bool? GestionDelai { get; set; }
        public int? LimiteAvertissement { get; set; }
    }
}