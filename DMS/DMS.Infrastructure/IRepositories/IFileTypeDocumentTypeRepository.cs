using DMS.DTO.DTOs;
using DMS.Entities.Models;
using Kendo.Mvc.UI;
using Master.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface IFileTypeDocumentTypeRepository<TEntity> : IGenericBaseRepository<TEntity>
        where TEntity : GedDocumentType
    {
        Task<TnTypesDossier?> FindFileTypeAsync(string fileTypeCode);
        Task<Guid?> FindFallbackTenantAsync();

        // Types de documents déjà rattachés à ce type de dossier
        Task<List<GedDocumentTypeDto>> GetAttachedTypesAsync(string fileTypeCode);

        // Types master data NON encore rattachés (candidats), filtrés/paginés côté serveur via Kendo DataSourceRequest
        Task<DataSourceResult> SearchAvailableTypesAsync(string fileTypeCode, DataSourceRequest request);

        Task<List<GedDocumentType>> GetTypesByIdsAsync(List<int> typeIds);

        // Lien "type ↔ catégorie" (GedCategorieType), point de passage vers le mapping dossier
        Task<GedCategorieType?> FindCategorieTypeAsync(int typeId, int categorieId);
        void AddCategorieType(GedCategorieType entity);

        Task<bool> MappingExistsAsync(int categorieTypeId, string fileTypeCode);
        void AddMapping(GedCategorieTypeTypesDossier mapping);

        Task<List<GedCategorieTypeTypesDossier>> GetMappingsForTypeAsync(string fileTypeCode, int typeId);
        void RemoveMappings(IEnumerable<GedCategorieTypeTypesDossier> mappings);

        // Commit unique avec positionnement du tenant PostgreSQL (RLS)
        Task SaveChangesAsync();
    }
}
