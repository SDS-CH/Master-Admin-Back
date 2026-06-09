#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Config;

namespace Master.EFCore.Repositories.Config
{
    public class MasterErpmailTemplateRepository : GenericBaseRepository<MasterErpMailTemplates, ERPMasterContext>, IMasterErpmailTemplateRepository<MasterErpMailTemplates>
    {
        public MasterErpmailTemplateRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }
    }
}
