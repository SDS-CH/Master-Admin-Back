#nullable disable
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.DTO.DTOs;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class CurrencyRepository : GenericBaseRepository<Currency, ERPMasterContext>, ICurrencyRepository<Currency>
    {
        public CurrencyRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllCurrencies(DataSourceRequest requestModel)
        {
            //return await dbContext.Currency.ToDataSourceResultAsync(requestModel);


            var res= await( from c in dbContext.Currency
                select new Currency
                {Id = c.Id,
                    Code = c.Code,Label = c.Label
                }).ToDataSourceResultAsync(requestModel);
            return res;
        }
    }
}
