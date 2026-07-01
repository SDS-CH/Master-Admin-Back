using DMS.DTO.DTOs;

namespace DMS.Infrastructure.IServices
{
    public interface IActivityService<TEntityDTO> where TEntityDTO : ActivityDto
    {
        Task<IEnumerable<TEntityDTO>> GetAll(int? industryId = null, bool unassignedOnly = false);
        Task Add(TEntityDTO dto);
        Task<bool> Update(string codeActivite, Guid tenantId, TEntityDTO dto);
        Task<bool> Delete(string codeActivite, Guid tenantId);
    }
}
