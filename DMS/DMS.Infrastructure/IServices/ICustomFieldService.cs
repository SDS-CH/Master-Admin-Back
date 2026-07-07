using DMS.DTO.DTOs;
using Master.Common.Interfaces.Services;

namespace DMS.Infrastructure.IServices
{
    public interface ICustomFieldService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : CustomFieldDto
    {
        Task<List<CustomFieldDto>> GetAllAsync();
        Task<List<CustomFieldDto>> GetByFileTypeAsync(string codeTypeDossier);
        List<string> GetAvailableTypes();
        Task CreateAsync(CreateCustomFieldDto dto);
        Task LinkAsync(LinkCustomFieldDto dto);
        Task UnlinkAsync(string codeTypeDossier, string complementCode);
        Task<bool> UpdateIsRequiredAsync(string codeTypeDossier, string complementCode, bool isRequiredOnFileClosure);
    }
}