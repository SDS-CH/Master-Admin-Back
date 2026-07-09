#nullable disable
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class ErpTenantRepository : GenericBaseRepository<ErpTenants, ERPMasterContext>, IErpTenantRepository<ErpTenants>
    {
        public ErpTenantRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllTenants(DataSourceRequest requestModel)
        {
            var query = from t in dbContext.ErpTenants
                        join d in dbContext.DbInstance on t.DbInstanceId equals d.Id into dj
                        from d in dj.DefaultIfEmpty()
                        select new
                        {
                            t.Id,
                            t.TenantId,
                            t.EntityName,
                            AddNewTime = t.AddNewTime.ToDateTime(TimeOnly.MinValue),
                            t.DbInstanceId,
                            DbInstanceName = d != null ? d.ServerName : null,
                            DbName = d != null ? d.DbName : null,
                        };

            return await query.ToDataSourceResultAsync(requestModel);
        }

        public async Task<List<string>> GetCryptedConnections()
        {
            var tenants = await dbContext.ErpTenants
                .Include(t => t.DbInstance)
                .Where(t => t.DbInstance != null)
                .ToListAsync();

            var allCs = string.Join(";", tenants.Select(t =>
                $"Host={t.DbInstance.ServerAddress};Database={t.EntityName};Username={t.DbInstance.DbUser};Password={t.DbInstance.Password}"));

            var firstCs = tenants.Any()
                ? $"Host={tenants[0].DbInstance.ServerAddress};Database={tenants[0].EntityName};Username={tenants[0].DbInstance.DbUser};Password={tenants[0].DbInstance.Password}"
                : string.Empty;

            return new List<string>
            {
                SimpleEncrypt(allCs),
                SimpleEncrypt(firstCs)
            };
        }

        public async Task<string> GetCryptedConnectionsByTenantId(int tenantId)
        {
            var tenant = await dbContext.ErpTenants
                .Include(t => t.DbInstance)
                .FirstOrDefaultAsync(t => t.Id == tenantId);

            if (tenant == null || tenant.DbInstance == null)
                return string.Empty;

            var cs = $"Host={tenant.DbInstance.ServerAddress};Database={tenant.EntityName};Username={tenant.DbInstance.DbUser};Password={tenant.DbInstance.Password}";
            return SimpleEncrypt(cs);
        }

        public async Task<ErpTenants> GetByTenantGuid(Guid tenantGuid)
        {
            return await dbContext.ErpTenants
                .FirstOrDefaultAsync(t => t.TenantId == tenantGuid);
        }

        private string SimpleEncrypt(string value) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(value ?? string.Empty));
    }
}
