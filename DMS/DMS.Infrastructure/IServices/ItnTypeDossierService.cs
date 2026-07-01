using DMS.DTO.DTOs;
using Master.Common.Interfaces.Services;

namespace DMS.Infrastructure.IServices
{
    public interface IFileTypeService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : FileTypeDto
    {
        Task<List<FileTypeDto>> GetByIndustryAsync(int industryId);
        Task<List<FileTypeDto>> GetSharedAsync();
        Task<FileTypeDto?> GetByCodeAsync(string codeTypeDossier);
        Task AddAsync(FileTypeDto dto);
        Task UpdateAsync(string codeTypeDossier, FileTypeDto dto);
        Task DeleteAsync(string codeTypeDossier);
    }
}