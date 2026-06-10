#nullable disable
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class MasterErpmoduleClientRepository : GenericBaseRepository<MasterErpModuleClient, ERPMasterContext>, IMasterErpmoduleClientRepository<MasterErpModuleClient>
    {
        public MasterErpmoduleClientRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId)
        {
            return await dbContext.MasterErpModuleClient
                .Where(x => x.TenantId == tenantId)
                .ToDataSourceResultAsync(requestModel);
        }

        public async Task<List<MasterErpModuleClient>> GetByTenantId(int tenantId)
        {
            return await dbContext.MasterErpModuleClient
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<int> DeleteByTenantId(int tenantId)
        {
            var items = dbContext.MasterErpModuleClient.Where(x => x.TenantId == tenantId);
            dbContext.MasterErpModuleClient.RemoveRange(items);
            return await dbContext.SaveChangesAsync();
        }
    }
}
