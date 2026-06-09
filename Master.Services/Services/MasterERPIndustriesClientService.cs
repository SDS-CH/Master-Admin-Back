#nullable disable
using AutoMapper;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Master.Infrastructure.IServices;
using System;
using System.Threading.Tasks;

namespace Master.Services.Services
{
    public class MasterERPIndustriesClientService<TMasterERPIndustriesClientDTO, TMasterErpIndustriesClient, TContext> : BaseService<TMasterERPIndustriesClientDTO, TMasterErpIndustriesClient, TContext>, IMasterERPIndustriesClientService<TMasterERPIndustriesClientDTO>
        where TMasterErpIndustriesClient : MasterErpIndustriesClient, new()
        where TMasterERPIndustriesClientDTO : MasterERPIndustriesClientDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterERPIndustriesClientRepository<TMasterErpIndustriesClient> _repository;

        public MasterERPIndustriesClientService(TContext dbContext, IMapper mapper, IMasterERPIndustriesClientRepository<TMasterErpIndustriesClient> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterERPIndustriesClientDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateIndustryClient(TMasterERPIndustriesClientDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "IndustryClient created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditIndustryClient(TMasterERPIndustriesClientDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "IndustryClient updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveIndustryClient(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "IndustryClient removed successfully.");
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
    }
}
