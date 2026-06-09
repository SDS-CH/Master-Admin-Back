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
using System.Threading.Tasks;

namespace Master.Services.Services
{
    public class ErpUserTenantService<TErpUserTenantDTO, TErpUserTenant, TContext> : BaseService<TErpUserTenantDTO, TErpUserTenant, TContext>, IErpUserTenantService<TErpUserTenantDTO>
        where TErpUserTenant : ErpUserTenants, new()
        where TErpUserTenantDTO : ErpUserTenantDTO
        where TContext : ERPMasterContext
    {
        private readonly IErpUserTenantRepository<TErpUserTenant> _repository;

        public ErpUserTenantService(TContext dbContext, IMapper mapper, IErpUserTenantRepository<TErpUserTenant> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TErpUserTenantDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> Create(TErpUserTenantDTO entity)
        {
            try
            {
                await base.Create(entity, _repository);
                return new OperationResult(false, "UserTenant created.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> Edit(TErpUserTenantDTO entity)
        {
            try
            {
                await base.Edit(entity, _repository);
                return new OperationResult(false, "UserTenant updated.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAll(DataSourceRequest requestModel)
        {
            return await _repository.GetAllUserTenants(requestModel);
        }

        public async Task<OperationResult> Remove(int userId, int tenantId)
        {
            try
            {
                var userTenant = await _dbContext.ErpUserTenants
                    .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);

                if (userTenant != null)
                {
                    _dbContext.ErpUserTenants.Remove(userTenant);
                    await _dbContext.SaveChangesAsync();
                }
                return new OperationResult(false, "UserTenant removed.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }
    }
}
