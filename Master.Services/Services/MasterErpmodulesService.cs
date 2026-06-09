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
using System.Threading.Tasks;

namespace Master.Services.Services
{
    public class MasterErpmodulesService<TMasterErpmodulesDTO, TMasterErpModules, TContext> : BaseService<TMasterErpmodulesDTO, TMasterErpModules, TContext>, IMasterErpmodulesService<TMasterErpmodulesDTO>
        where TMasterErpModules : MasterErpModules, new()
        where TMasterErpmodulesDTO : MasterErpmodulesDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterErpmodulesRepository<TMasterErpModules> _repository;

        public MasterErpmodulesService(TContext dbContext, IMapper mapper, IMasterErpmodulesRepository<TMasterErpModules> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterErpmodulesDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateModule(TMasterErpmodulesDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "Module created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditModule(TMasterErpmodulesDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "Module updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveModule(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "Module removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllModules(DataSourceRequest requestModel)
        {
            return await _repository.GetAll(requestModel);
        }
    }
}
