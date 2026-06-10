#nullable disable
using AutoMapper;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO.DTOs;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Master.Infrastructure.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Services.Services
{
    public class ErpUserService<TErpUserDTO, TErpUser, TContext> : BaseService<TErpUserDTO, TErpUser, TContext>, IErpUserService<TErpUserDTO>
        where TErpUser : ErpUsers, new()
        where TErpUserDTO : ErpUserDTO
        where TContext : ERPMasterContext
    {
        private readonly IErpUserRepository<TErpUser> _userRepository;
        private readonly IErpUserTenantRepository<ErpUserTenants> _userTenantRepository;

        public ErpUserService(TContext dbContext, IMapper mapper, IErpUserRepository<TErpUser> userRepository, IErpUserTenantRepository<ErpUserTenants> userTenantRepository)
            : base(dbContext, mapper)
        {
            _userRepository = userRepository;
            _userTenantRepository = userTenantRepository;
        }

        public async Task<TErpUserDTO> GetById(int id)
        {
            return await base.GetById(id, _userRepository);
        }

        public async Task<DataSourceResult> GetAllUsers(DataSourceRequest requestModel)
        {
            return await _userRepository.GetAll(requestModel);
        }

        public async Task<OperationResult> CreateUser(TErpUserDTO entity)
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

        public async Task<OperationResult> EditUser(TErpUserDTO entity, int id)
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

        public async Task<ErpUserDTO> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<ErpUserDTO> GetByUserName(string email)
        {
            return await _userRepository.GetByUserName(email);
        }

        public async Task<DataSourceResult> GetUsersByTenant(DataSourceRequest requestModel, int tenantId)
        {
            return await _userRepository.GetUsersByTenant(requestModel, tenantId);
        }

        public async Task<List<ErpUserDTO>> GetErpUsersByTenant(int tenantId)
        {
            return await _userRepository.GetErpUsersByTenant(tenantId);
        }

        public async Task<OperationResult> VerifEmailExist(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user != null)
                return new OperationResult(true, "Email already exists.");
            return new OperationResult(false, "Email is available.");
        }

        public async Task<OperationResult> CreateUserForTenant(TErpUserDTO entity, int tenantId)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.Password))
                    entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);

                var created = await Create(entity, _userRepository);
                var userTenant = new ErpUserTenants
                {
                    UserId = created.Id,
                    TenantId = tenantId,
                    IsActive = true,
                    IsDefault = true,
                    IsErpUser = true,
                    AddNewTime = DateOnly.FromDateTime(DateTime.UtcNow)
                };
                await _userTenantRepository.Create(userTenant);
                return new OperationResult(false, "User created and assigned to tenant.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> CreateOrAttachUserForTenant(TErpUserDTO entity, int tenantId)
        {
            try
            {
                var existing = await _userRepository.GetByEmail(entity.Email);
                if (existing != null)
                {
                    var existingTenant = await _userTenantRepository.GetByUserId(existing.Id);
                    if (existingTenant == null)
                    {
                        var userTenant = new ErpUserTenants
                        {
                            UserId = existing.Id,
                            TenantId = tenantId,
                            IsActive = true,
                            IsDefault = true,
                            IsErpUser = true,
                            AddNewTime = DateOnly.FromDateTime(DateTime.UtcNow)
                        };
                        await _userTenantRepository.Create(userTenant);
                    }
                    return new OperationResult(false, "User attached to tenant.");
                }

                return await CreateUserForTenant(entity, tenantId);
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> DeactivateUserForTenant(int userId, int tenantId)
        {
            try
            {
                var userTenant = await _dbContext.ErpUserTenants
                    .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);

                if (userTenant != null)
                {
                    userTenant.IsActive = false;
                    await _dbContext.SaveChangesAsync();
                }
                return new OperationResult(false, "User deactivated for tenant.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> ActivateUserForTenant(TErpUserDTO entity, int tenantId)
        {
            try
            {
                var userTenant = await _dbContext.ErpUserTenants
                    .FirstOrDefaultAsync(ut => ut.UserId == entity.Id && ut.TenantId == tenantId);

                if (userTenant != null)
                {
                    userTenant.IsActive = true;
                    await _dbContext.SaveChangesAsync();
                }
                return new OperationResult(false, "User activated for tenant.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }
    }
}
