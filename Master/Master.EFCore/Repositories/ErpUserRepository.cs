#nullable disable
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Master.DTO.DTOs;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class ErpUserRepository : GenericBaseRepository<ErpUsers, ERPMasterContext>, IErpUserRepository<ErpUsers>
    {
        private readonly IMapper _mapper;

        public ErpUserRepository(ERPMasterContext context, IMapper mapper) : base(context)
        {
            dbContext = context;
            _mapper = mapper;
        }

        public async Task<ErpUserDTO> GetByEmail(string email)
        {
            var user = await dbContext.ErpUsers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return _mapper.Map<ErpUserDTO>(user);
        }

        public async Task<List<ErpUserDTO>> GetAccountsByEmail(string email)
        {
            var users = await dbContext.ErpUsers
                .Where(u => u.Email.ToLower() == email.ToLower())
                .ToListAsync();
            return _mapper.Map<List<ErpUserDTO>>(users);
        }

        public async Task<ErpUserDTO> GetByUserName(string userName)
        {
            var user = await dbContext.ErpUsers
                .FirstOrDefaultAsync(u => u.UserName == userName);
            return _mapper.Map<ErpUserDTO>(user);
        }

        public async Task<DataSourceResult> GetUsersByTenant(DataSourceRequest requestModel, int tenantId)
        {
            var query = from ut in dbContext.ErpUserTenants
                        join u in dbContext.ErpUsers on ut.UserId equals u.Id
                        where ut.TenantId == tenantId
                        select new ErpUserDTO
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            IsActive = ut.IsActive,         
                            IsErpUser = ut.IsErpUser,       

                            Tenants = dbContext.ErpUserTenants
                                               .Where(t => t.UserId == u.Id)
                                               .Select(t => new ErpUserTenantDTO
                                               {
                                                   TenantId = t.TenantId,
                                                   IsActive = t.IsActive
                                               }).ToList()
                        };

            return await query.ToDataSourceResultAsync(requestModel);
        }

        public async Task<List<ErpUserDTO>> GetErpUsersByTenant(int tenantId)
        {
            var query = from u in dbContext.ErpUsers
                        join ut in dbContext.ErpUserTenants on u.Id equals ut.UserId
                        where ut.TenantId == tenantId
                        select new ErpUserDTO
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            IsActive = u.IsActive,
                            IsErpUser = ut.IsErpUser
                        };

            return await query.ToListAsync();
        }

        public async Task<int> IncrementLoginAttempts(string email, bool blockAccount, int? blockAfterAttempts)
        {
            if (blockAccount)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(
                    "UPDATE erpmaster.erp_users SET failed_login_attempts = failed_login_attempts + 1, is_blocked = true, last_attempts = NOW() WHERE email = {0}",
                    email);
            }
            else
            {
                return await dbContext.Database.ExecuteSqlRawAsync(
                    "UPDATE erpmaster.erp_users SET failed_login_attempts = failed_login_attempts + 1, last_attempts = NOW() WHERE email = {0}",
                    email);
            }
        }

        public async Task<int> ResetLoginAttempts(string email)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(
                "UPDATE erpmaster.erp_users SET failed_login_attempts = 0, is_blocked = false, last_attempts = NULL WHERE email = {0}",
                email);
        }
    }
}
