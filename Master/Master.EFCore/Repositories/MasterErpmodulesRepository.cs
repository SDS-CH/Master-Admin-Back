#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;

namespace Master.EFCore.Repositories
{
    public class MasterErpmodulesRepository : GenericBaseRepository<MasterErpModules, ERPMasterContext>, IMasterErpmodulesRepository<MasterErpModules>
    {
        public MasterErpmodulesRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }
    }
}
