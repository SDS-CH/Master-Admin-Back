#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;

namespace Master.EFCore.Repositories
{
    public class DbInstanceRepository : GenericBaseRepository<DbInstance, ERPMasterContext>, IDbInstanceRepository<DbInstance>
    {
        public DbInstanceRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }
    }
}
