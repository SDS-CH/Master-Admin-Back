using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IErpUserTenantService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : ErpUserTenantDTO
    {
        Task<OperationResult> Create(TEntityDTO entity);
        Task<OperationResult> Edit(TEntityDTO entity);
        Task<DataSourceResult> GetAll(DataSourceRequest requestModel);
        Task<OperationResult> Remove(int userId, int tenantId);
    }
}
