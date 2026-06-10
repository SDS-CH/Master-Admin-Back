#nullable disable
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class ErpUserTenantRepository : GenericBaseRepository<ErpUserTenants, ERPMasterContext>, IErpUserTenantRepository<ErpUserTenants>
    {
        public ErpUserTenantRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllUserTenants(DataSourceRequest requestModel)
        {
            return await dbContext.ErpUserTenants.ToDataSourceResultAsync(requestModel);
        }

        public async Task<ErpUserTenants> GetByUserId(int userId)
        {
            return await dbContext.ErpUserTenants
                .FirstOrDefaultAsync(ut => ut.UserId == userId);
        }
    }
}
