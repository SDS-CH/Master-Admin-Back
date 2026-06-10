#nullable disable
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class MasterERPIndustriesRepository : GenericBaseRepository<MasterErpIndustries, ERPMasterContext>, IMasterERPIndustriesRepository<MasterErpIndustries>
    {
        public MasterERPIndustriesRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllIndustries(DataSourceRequest requestModel)
        {
            return await dbContext.MasterErpIndustries.ToDataSourceResultAsync(requestModel);
        }
    }
}
