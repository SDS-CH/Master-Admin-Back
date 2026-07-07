using DMS.DTO.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IServices
{
    public interface IFileTypeMilestonesService
    {
        Task<List<FileTypeMilestoneDto>?> GetMappedMilestonesAsync(string fileTypeCode);
        Task<List<MilestoneStepDto>?> SearchMilestonesAsync(string fileTypeCode, string? search);
        Task<string> AddMilestonesAsync(string fileTypeCode, AddFileTypeMilestonesDto dto);
        Task<MilestoneStepDto> CreateStepAsync(string fileTypeCode, CreateMilestoneStepDto dto);
        Task<bool> UpdateMilestoneMappingAsync(int mappingId, UpdateFileTypeMilestoneDto dto);
        Task<bool> DeleteMilestoneMappingAsync(int mappingId);
    }
}
