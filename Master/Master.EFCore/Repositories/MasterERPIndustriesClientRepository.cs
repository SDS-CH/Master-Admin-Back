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
    public class MasterERPIndustriesClientRepository : GenericBaseRepository<MasterErpIndustriesClient, ERPMasterContext>, IMasterERPIndustriesClientRepository<MasterErpIndustriesClient>
    {
        public MasterERPIndustriesClientRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId)
        {
            return await dbContext.MasterErpIndustriesClients
                .Where(x => x.TenantId == tenantId)
                .ToDataSourceResultAsync(requestModel);
        }

        public async Task<List<MasterErpIndustriesClient>> GetByTenantId(int tenantId)
        {
            return await dbContext.MasterErpIndustriesClients
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();
        }
    }
}
