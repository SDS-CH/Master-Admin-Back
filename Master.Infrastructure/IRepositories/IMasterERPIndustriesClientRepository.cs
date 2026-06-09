using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IMasterERPIndustriesClientRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpIndustriesClient
    {
        Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId);
        Task<List<MasterErpIndustriesClient>> GetByTenantId(int tenantId);
    }
}
