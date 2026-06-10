using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IMasterERPDatabaseRefRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpDatabaseRef
    {
        Task<DataSourceResult> GetAllDatabaseRefs(DataSourceRequest requestModel);
    }
}
