using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.Config;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices.Config
{
    public interface IMasterErpmailTemplateService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterErpmailTemplateDTO
    {
        Task<OperationResult> CreateMailTemplate(TEntityDTO entity);
        Task<OperationResult> EditMailTemplate(TEntityDTO entity, int id);
        Task<OperationResult> RemoveMailTemplate(int id);
        Task<DataSourceResult> GetAllMailTemplates(DataSourceRequest requestModel);
    }
}
