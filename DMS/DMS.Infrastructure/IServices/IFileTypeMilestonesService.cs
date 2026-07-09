using DMS.DTO.DTOs;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Interfaces.Services;

namespace DMS.Infrastructure.IServices
{
    public interface IFileTypeMilestonesService<TEntityDTO> : IBaseService<TEntityDTO>
        where TEntityDTO : MilestoneStepDto
    {
        // Jalons déjà mappés au type de dossier
        Task<DataSourceResult?> GetAllMappedMilestonesAsync(string fileTypeCode, DataSourceRequest requestModel);
        Task<List<FileTypeMilestoneDto>?> GetMappedMilestonesAsync(string fileTypeCode);

        // Recherche de jalons disponibles (candidats à mapper)
        Task<DataSourceResult?> SearchMilestonesAsync(string fileTypeCode, DataSourceRequest requestModel, MilestoneStepDto filter);
        Task<List<MilestoneStepDto>?> SearchMilestonesAsync(string fileTypeCode, string? search);

        // Ecritures
        Task<OperationResult> AddMilestonesAsync(string fileTypeCode, AddFileTypeMilestonesDto dto);
        Task<OperationResult> CreateStepAsync(string fileTypeCode, CreateMilestoneStepDto dto);
        Task<OperationResult> UpdateMilestoneMappingAsync(int mappingId, UpdateFileTypeMilestoneDto dto);
        Task<OperationResult> DeleteMilestoneMappingAsync(int mappingId);
    }
}
