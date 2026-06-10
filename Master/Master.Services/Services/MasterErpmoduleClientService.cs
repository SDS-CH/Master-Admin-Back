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
    public class MasterErpmoduleClientService<TMasterErpmoduleClientDTO, TMasterErpModuleClient, TContext> : BaseService<TMasterErpmoduleClientDTO, TMasterErpModuleClient, TContext>, IMasterErpmoduleClientService<TMasterErpmoduleClientDTO>
        where TMasterErpModuleClient : MasterErpModuleClient, new()
        where TMasterErpmoduleClientDTO : MasterErpmoduleClientDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterErpmoduleClientRepository<TMasterErpModuleClient> _repository;

        public MasterErpmoduleClientService(TContext dbContext, IMapper mapper, IMasterErpmoduleClientRepository<TMasterErpModuleClient> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterErpmoduleClientDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateModuleClient(TMasterErpmoduleClientDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "ModuleClient created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditModuleClient(TMasterErpmoduleClientDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "ModuleClient updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveModuleClient(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "ModuleClient removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId)
        {
            return await _repository.GetAllByTenant(requestModel, tenantId);
        }

        public async Task<List<MasterErpmoduleClientDTO>> GetByTenantId(int tenantId)
        {
            var items = await _repository.GetByTenantId(tenantId);
            return _mapper.Map<List<MasterErpmoduleClientDTO>>(items);
        }

        public async Task<OperationResult> DeleteByTenantId(int tenantId)
        {
            try
            {
                await _repository.DeleteByTenantId(tenantId);
                return new OperationResult(false, "ModuleClients deleted for tenant.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }
    }
}
