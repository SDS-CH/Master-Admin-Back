using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IMasterErpmoduleClientRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpModuleClient
    {
        Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId);
        Task<List<MasterErpModuleClient>> GetByTenantId(int tenantId);
        Task<int> DeleteByTenantId(int tenantId);
    }
}
