using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
namespace Master.Infrastructure.IRepositories

{

    public interface IErpCountryRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : ErpCountries

    {

        Task<DataSourceResult> GetAllCountries(DataSourceRequest requestModel);

    }

}