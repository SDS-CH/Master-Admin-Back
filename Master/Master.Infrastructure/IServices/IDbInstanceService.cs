using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IDbInstanceService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : DbInstanceDTO
    {
        Task<OperationResult> CreateDbInstance(TEntityDTO entity);
        Task<OperationResult> EditDbInstance(TEntityDTO entity, int id);
        Task<OperationResult> RemoveDbInstance(int id);
        Task<DataSourceResult> GetAllDbInstances(DataSourceRequest requestModel);
    }
}
