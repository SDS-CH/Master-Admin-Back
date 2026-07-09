using System.Collections.Generic;

namespace DMS.DTO.DTOs
{
    // Payload d'attachement de types de documents (master data) à un type de dossier.
    public class AttachDocumentTypesDto
    {
        public List<int> TypeIds { get; set; } = new List<int>();
    }
}
