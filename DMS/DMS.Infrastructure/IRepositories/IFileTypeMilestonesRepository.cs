using DMS.DTO.DTOs;
using DMS.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface IFileTypeMilestonesRepository
    {
        Task<TnTypesDossier?> FindFileTypeAsync(string fileTypeCode);
        Task<List<FileTypeMilestoneDto>> GetMappedMilestonesAsync(TnTypesDossier fileType);
        Task<List<MilestoneStepDto>> SearchMilestonesForFileTypeAsync(TnTypesDossier fileType, string? search);
        Task<Guid?> FindFallbackTenantAsync(string normalizedCode);
        Task<List<TnCodesEtape>> GetStepsByCodesAsync(List<string> stepCodes, Guid tenantId);
        Task<List<string>> GetExistingMappingStepCodesAsync(string fileTypeCode, Guid tenantId);
        Task<int> GetMaxMappingOrderAsync(string fileTypeCode, Guid tenantId);
        Task AddFileTypeStepsAsync(List<TnFileTypeStep> mappings);
        Task<int> GetMaxStepOrderAsync(Guid tenantId);
        Task AddStepAsync(TnCodesEtape step);
        Task AddFileTypeStepAsync(TnFileTypeStep mapping);
        Task<TnFileTypeStep?> GetMappingByIdAsync(int mappingId);
        Task<bool> StepCodeExistsAsync(string code, Guid tenantId);
        void RemoveFileTypeStep(TnFileTypeStep mapping);
        Task SaveChangesAsync();
    }
}
