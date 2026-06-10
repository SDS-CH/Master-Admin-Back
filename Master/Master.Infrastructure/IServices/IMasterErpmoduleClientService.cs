using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IMasterErpmoduleClientService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterErpmoduleClientDTO
    {
        Task<OperationResult> CreateModuleClient(TEntityDTO entity);
        Task<OperationResult> EditModuleClient(TEntityDTO entity, int id);
        Task<OperationResult> RemoveModuleClient(int id);
        Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId);
        Task<List<MasterErpmoduleClientDTO>> GetByTenantId(int tenantId);
        Task<OperationResult> DeleteByTenantId(int tenantId);
    }
}
