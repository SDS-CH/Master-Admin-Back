using Master.Common.Interfaces;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface ICurrencyRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : Currency
    {
        Task<DataSourceResult> GetAllCurrencies(DataSourceRequest requestModel);
    }
}
