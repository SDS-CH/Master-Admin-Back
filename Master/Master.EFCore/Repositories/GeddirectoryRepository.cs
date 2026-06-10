#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;

namespace Master.EFCore.Repositories
{
    public class GeddirectoryRepository : GenericBaseRepository<GedDirectory, ERPMasterContext>, IGedDirectoryRepository<GedDirectory>
    {
        public GeddirectoryRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }
    }
}
