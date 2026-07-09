using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IErpUserService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : ErpUserDTO
    {
        Task<DataSourceResult> GetAllUsers(DataSourceRequest requestModel);
        Task<OperationResult> CreateUser(TEntityDTO entity);
        Task<OperationResult> EditUser(TEntityDTO entity, int id);
        Task<ErpUserDTO> GetByEmail(string email);
        Task<ErpUserDTO> GetByUserName(string email);
        Task<OperationResult> RemoveUser(int id);
        Task<DataSourceResult> GetUsersByTenant(DataSourceRequest requestModel, int tenantId);
        Task<List<ErpUserDTO>> GetErpUsersByTenant(int tenantId);
        Task<OperationResult> VerifEmailExist(string email);
        Task<OperationResult> CreateUserForTenant(TEntityDTO entity, int tenantId);
        Task<OperationResult> CreateOrAttachUserForTenant(TEntityDTO entity, int tenantId);
        Task<OperationResult> DeactivateUserForTenant(int userId, int tenantId);
        Task<OperationResult> ActivateUserForTenant(int userId, int tenantId);
    }
}
