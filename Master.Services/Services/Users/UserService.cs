#nullable disable
using AutoMapper;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO.Users;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Users;
using Master.Infrastructure.IServices.Users;
using System;
using System.Threading.Tasks;

namespace Master.Services.Services.Users
{
    public class UserService<TUserDTO, TUser, TContext> : BaseService<TUserDTO, TUser, TContext>, IUserService<TUserDTO>
        where TUser : MasterAdminUsers, new()
        where TUserDTO : MasterAdminUsersDTO
        where TContext : ERPMasterContext
    {
        private readonly IUserRepository<TUser> _userRepository;

        public UserService(TContext dbContext, IMapper mapper, IUserRepository<TUser> userRepository)
            : base(dbContext, mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<TUserDTO> GetById(int id)
        {
            return await base.GetById(id, _userRepository);
        }

        public async Task<OperationResult> CreateUser(TUserDTO entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.Password))
                    entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);

                await Create(entity, _userRepository);
                return new OperationResult(false, "User created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditUser(TUserDTO entity, int id)
        {
            try
            {
                await Edit(entity, _userRepository);
                return new OperationResult(false, "User updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveUser(int id)
        {
            try
            {
                await Delete(id, _userRepository);
                return new OperationResult(false, "User removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }
    }
}
