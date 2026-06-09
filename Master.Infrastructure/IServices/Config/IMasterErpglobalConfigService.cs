using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.Config;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices.Config
{
    public interface IMasterErpglobalConfigService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterErpglobalConfigDTO
    {
        Task<OperationResult> CreateConfig(TEntityDTO entity);
        Task<OperationResult> EditConfig(TEntityDTO entity, int id);
        Task<MasterErpglobalConfigDTO> GetFirstConfig();
        Task<DataSourceResult> GetAll(DataSourceRequest request);
    }
}
