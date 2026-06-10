using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IErpTenantService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : ErpTenantDTO
    {
        Task<OperationResult> CreateTenant(TEntityDTO tenant);
        Task<DataSourceResult> GetAllTenants(DataSourceRequest requestModel);
        Task<OperationResult> EditTenant(TEntityDTO entity, int id);
        Task<OperationResult> RemoveTenant(int id);
        Task<List<string>> GetCryptedConnections();
        Task<string> GetCryptedConnectionsByTenantId(int tenantId);
    }
}
