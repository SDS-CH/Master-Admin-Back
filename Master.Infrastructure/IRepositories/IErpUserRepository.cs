using Master.Common.Interfaces;
using Master.DTO.DTOs;
using Master.Entities.Models;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IErpUserRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : ErpUsers
    {
        Task<ErpUserDTO> GetByEmail(string email);
        Task<List<ErpUserDTO>> GetAccountsByEmail(string email);
        Task<ErpUserDTO> GetByUserName(string userName);
        Task<DataSourceResult> GetUsersByTenant(DataSourceRequest requestModel, int tenantId);
        Task<List<ErpUserDTO>> GetErpUsersByTenant(int tenantId);
        Task<int> IncrementLoginAttempts(string email, bool blockAccount, int? blockAfterAttempts);
        Task<int> ResetLoginAttempts(string email);
    }
}
