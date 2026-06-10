#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories.Users
{
    public class UserRepository : GenericBaseRepository<MasterAdminUsers, ERPMasterContext>, IUserRepository<MasterAdminUsers>
    {
        public UserRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<MasterAdminUsers> GetByUserNameSimple(string username)
        {
            return await dbContext.MasterAdminUsers
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<string> GetMailByUserName(object userName)
        {
            return await dbContext.MasterAdminUsers
                .Where(u => u.UserName == userName.ToString())
                .Select(u => u.Email)
                .FirstOrDefaultAsync();
        }
    }
}
