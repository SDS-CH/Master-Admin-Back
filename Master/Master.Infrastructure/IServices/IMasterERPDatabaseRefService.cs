using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IMasterERPDatabaseRefService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterERPDatabaseRefDTO
    {
        Task<OperationResult> CreateDatabaseRef(TEntityDTO entity);
        Task<OperationResult> EditDatabaseRef(TEntityDTO entity, int id);
        Task<OperationResult> RemoveDatabaseRef(int id);
        Task<DataSourceResult> GetAllDatabaseRefs(DataSourceRequest requestModel);
    }
}
