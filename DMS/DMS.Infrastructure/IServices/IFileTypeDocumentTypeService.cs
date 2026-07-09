using DMS.DTO.DTOs;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IServices
{
    public interface IFileTypeDocumentTypeService<TEntityDTO>
        where TEntityDTO : GedDocumentTypeDto
    {
        // Types déjà rattachés au type de dossier (grille)
        Task<List<GedDocumentTypeDto>> GetAttachedTypesAsync(string fileTypeCode);

        // Recherche serveur (Kendo DataSourceRequest) des types disponibles à rattacher (multiselect)
        Task<DataSourceResult> SearchTypesAsync(string fileTypeCode, DataSourceRequest request);

        // Ecritures
        Task<OperationResult> AttachTypesAsync(string fileTypeCode, AttachDocumentTypesDto dto);
        Task<OperationResult> DetachTypeAsync(string fileTypeCode, int typeId);
    }
}
