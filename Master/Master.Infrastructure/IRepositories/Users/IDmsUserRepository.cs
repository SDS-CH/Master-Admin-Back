using Master.Common.Interfaces;
using Master.Entities.Models;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories.Users
{
    public interface IDmsUserRepository<TUser> : IGenericBaseRepository<TUser> where TUser : MasterAdminUsers
    {
        Task<TUser> GetByUserName(string username);
        Task<TUser> GetByEmail(string email);
    }
}
