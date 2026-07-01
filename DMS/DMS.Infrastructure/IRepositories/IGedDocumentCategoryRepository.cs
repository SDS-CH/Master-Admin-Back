using DMS.DTO.DTOs;
using DMS.Entities.Models;
using Master.Common.Interfaces;

namespace DMS.Infrastructure.IRepositories
{
    public interface IGedDocumentCategoryRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : GedDocumentCategory
    {
        Task<List<TEntity>> GetCategories();
        Task<IEnumerable<GedDocumentTypeDto>> GetTypes(int categoryId, int? industryId, bool unassignedOnly = false);
        Task<GedDocumentTypeDto> CreateType(GedDocumentTypeDto dto);
        Task<GedDocumentTypeDto?> UpdateType(int id, GedDocumentTypeDto dto);
        Task<bool> DeleteType(int id);
    }
}
