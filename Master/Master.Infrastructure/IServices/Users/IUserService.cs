using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.Users;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices.Users
{
    public interface IUserService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterAdminUsersDTO
    {
        Task<OperationResult> CreateUser(TEntityDTO entity);
        Task<OperationResult> EditUser(TEntityDTO entity, int id);
        Task<OperationResult> RemoveUser(int id);
    }
}
