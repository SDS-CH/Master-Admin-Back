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
    public class MasterERPIndustriesService<TMasterERPIndustriesDTO, TMasterErpIndustries, TContext> : BaseService<TMasterERPIndustriesDTO, TMasterErpIndustries, TContext>, IMasterERPIndustriesService<TMasterERPIndustriesDTO>
        where TMasterErpIndustries : MasterErpIndustries, new()
        where TMasterERPIndustriesDTO : MasterERPIndustriesDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterERPIndustriesRepository<TMasterErpIndustries> _repository;

        public MasterERPIndustriesService(TContext dbContext, IMapper mapper, IMasterERPIndustriesRepository<TMasterErpIndustries> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterERPIndustriesDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateIndustry(TMasterERPIndustriesDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "Industry created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditIndustry(TMasterERPIndustriesDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "Industry updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveIndustry(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "Industry removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllIndustries(DataSourceRequest requestModel)
        {
            return await _repository.GetAllIndustries(requestModel);
        }
    }
}
