#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories.Users
{
    public class DmsUserRepository : GenericBaseRepository<MasterAdminUsers, ERPMasterContext>, IDmsUserRepository<MasterAdminUsers>
    {
        public DmsUserRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<MasterAdminUsers> GetByUserName(string username)
        {
            return await dbContext.MasterAdminUsers
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }

        public async Task<MasterAdminUsers> GetByEmail(string email)
        {
            return await dbContext.MasterAdminUsers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
