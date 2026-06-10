using Master.Common.Interfaces;
using Master.Entities.Models;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories.Users
{
    public interface IUserRepository<TUser> : IGenericBaseRepository<TUser> where TUser : MasterAdminUsers
    {
        Task<MasterAdminUsers> GetByUserNameSimple(string username);
        Task<string> GetMailByUserName(object userName);
    }
}
