using DMS.DTO.DTOs;
using DMS.Entities.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface IFileTypeMilestonesRepository 
    {
        Task<TnTypesDossier?> FindFileTypeAsync(string fileTypeCode);

        // Equivalent à GetAllSteps : liste paginée/triée par le Kendo Grid
        Task<DataSourceResult> GetAllMappedMilestones(DataSourceRequest requestModel, TnTypesDossier fileType);

        Task<List<FileTypeMilestoneDto>> GetMappedMilestonesAsync(TnTypesDossier fileType);

        // Equivalent à ReadFileStepCode : recherche filtrée avec pagination réelle
        Task<DataSourceResult> SearchMilestonesForFileType(DataSourceRequest requestModel, TnTypesDossier fileType, MilestoneStepDto filter);

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