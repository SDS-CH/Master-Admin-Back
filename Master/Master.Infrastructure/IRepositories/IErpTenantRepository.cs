using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IErpTenantRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : ErpTenants
    {
        Task<DataSourceResult> GetAllTenants(DataSourceRequest requestModel);
        Task<List<string>> GetCryptedConnections();
        Task<string> GetCryptedConnectionsByTenantId(int tenantId);
        Task<ErpTenants> GetByTenantGuid(Guid tenantGuid);
    }
}
