using ERP.Common.Classes.CommonDTOs;
using ERP.Common.Interfaces.Services;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using OIC.DTO;
using OIC.DTO.File;
using OIC.DTO.MasterDataOperation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OIC.Infrastructure.IServices.MasterDataOperation
{
    public interface IStepsCodesService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : StepsCodesDTO
    {
        Task<DataSourceResult> ReadAllSteps(DataSourceRequest requestModel);
        Task<OperationResult> DeleteStepsCodes(string code);
        Task<OperationResult> CreateStep(StepsCodesDTO stepsEditorDTO);
        Task<OperationResult> EditStep(StepsCodesDTO stepsEditorDTO);
        Task<DataSourceResult> ReadFileStepCode([DataSourceRequest] DataSourceRequest requestModel, StepsCodesDTO fileFilter);

    }
}
