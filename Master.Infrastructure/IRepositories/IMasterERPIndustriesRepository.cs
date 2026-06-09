using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IMasterERPIndustriesRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpIndustries
    {
        Task<DataSourceResult> GetAllIndustries(DataSourceRequest requestModel);
    }
}
