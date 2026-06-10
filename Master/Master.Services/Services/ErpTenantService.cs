#nullable disable
using AutoMapper;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO.DTOs;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Master.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.Services.Services
{
    public class ErpTenantService<TErpTenantDTO, TErpTenant, TContext> : BaseService<TErpTenantDTO, TErpTenant, TContext>, IErpTenantService<TErpTenantDTO>
        where TErpTenant : ErpTenants, new()
        where TErpTenantDTO : ErpTenantDTO
        where TContext : ERPMasterContext
    {
        private readonly IErpTenantRepository<TErpTenant> _tenantRepository;

        public ErpTenantService(TContext dbContext, IMapper mapper, IErpTenantRepository<TErpTenant> repo)
            : base(dbContext, mapper)
        {
            _tenantRepository = repo;
        }

        public async Task<TErpTenantDTO> GetById(int id)
        {
            return await base.GetById(id, _tenantRepository);
        }

        public async Task<OperationResult> CreateTenant(TErpTenantDTO tenant)
        {
            try
            {
                await Create(tenant, _tenantRepository);
                return new OperationResult(false, "Tenant created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllTenants(DataSourceRequest requestModel)
        {
            return await _tenantRepository.GetAllTenants(requestModel);
        }

        public async Task<OperationResult> EditTenant(TErpTenantDTO entity, int id)
        {
            try
            {
                await Edit(entity, _tenantRepository);
                return new OperationResult(false, "Tenant updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveTenant(int id)
        {
            try
            {
                await Delete(id, _tenantRepository);
                return new OperationResult(false, "Tenant removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<List<string>> GetCryptedConnections()
        {
            return await _tenantRepository.GetCryptedConnections();
        }

        public async Task<string> GetCryptedConnectionsByTenantId(int tenantId)
        {
            return await _tenantRepository.GetCryptedConnectionsByTenantId(tenantId);
        }
    }
}
