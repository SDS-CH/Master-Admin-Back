#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Config;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories.Config
{
    public class MasterErpglobalConfigRepository : GenericBaseRepository<MasterErpGlobalConfigs, ERPMasterContext>, IMasterErpglobalConfigRepository<MasterErpGlobalConfigs>
    {
        public MasterErpglobalConfigRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<MasterErpGlobalConfigs> GetFirstConfig()
        {
            return await dbContext.MasterErpGlobalConfigs.FirstOrDefaultAsync();
        }
    }
}
