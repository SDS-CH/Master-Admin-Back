using DMS.DTO.DTOs;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Interfaces.Services;


namespace OIC.Infrastructure.IServices.MasterDataOperation
{
    public interface IFileTypeMilestonesService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : FileTypeDto
    {
        Task<DataSourceResult> ReadAllSteps(DataSourceRequest requestModel);
        Task<OperationResult> DeleteStepsCodes(string code);
        Task<OperationResult> CreateStep(FileTypeDto stepsEditorDTO);
        Task<OperationResult> EditStep(FileTypeDto stepsEditorDTO);
        Task<DataSourceResult> ReadFileStepCode([DataSourceRequest] DataSourceRequest requestModel, FileTypeDto fileFilter);

    }
}
