using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IMasterErpmodulesService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterErpmodulesDTO
    {
        Task<OperationResult> CreateModule(TEntityDTO entity);
        Task<OperationResult> EditModule(TEntityDTO entity, int id);
        Task<OperationResult> RemoveModule(int id);
        Task<DataSourceResult> GetAllModules(DataSourceRequest requestModel);
    }
}
