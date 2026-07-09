using DMS.DTO.DTOs;

namespace DMS.Infrastructure.IServices
{
    public interface IGedDocumentCategoryService<TEntityDTO> where TEntityDTO : GedDocumentCategoryDto
    {
        Task<IEnumerable<TEntityDTO>> GetCategories();
        Task<IEnumerable<GedDocumentTypeDto>> GetTypes(int categoryId, int? industryId, bool unassignedOnly = false);
        Task<(bool Success, string? Error, GedDocumentTypeDto? Data)> CreateType(GedDocumentTypeDto dto);
        Task<(bool Success, string? Error, GedDocumentTypeDto? Data)> UpdateType(int id, GedDocumentTypeDto dto);
        Task<bool> DeleteType(int id);
    }
}
