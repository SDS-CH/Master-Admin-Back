#nullable disable
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class MasterERPDatabaseRefRepository : GenericBaseRepository<MasterErpDatabaseRef, ERPMasterContext>, IMasterERPDatabaseRefRepository<MasterErpDatabaseRef>
    {
        public MasterERPDatabaseRefRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllDatabaseRefs(DataSourceRequest requestModel)
        {
            return await dbContext.MasterErpDatabaseRef.ToDataSourceResultAsync(requestModel);
        }
    }
}
