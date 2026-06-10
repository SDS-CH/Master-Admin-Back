using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IErpUserTenantRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : ErpUserTenants
    {
        Task<DataSourceResult> GetAllUserTenants(DataSourceRequest requestModel);
        Task<ErpUserTenants> GetByUserId(int userId);
    }
}
