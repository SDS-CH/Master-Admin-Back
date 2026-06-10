#nullable disable
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class ERPCountryRepository : GenericBaseRepository<ErpCountries, ERPMasterContext>, IErpCountryRepository<ErpCountries>
    {
        public ERPCountryRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllCountries(DataSourceRequest requestModel)
        {
            return await dbContext.ErpCountries.ToDataSourceResultAsync(requestModel);
        }
    }
}
