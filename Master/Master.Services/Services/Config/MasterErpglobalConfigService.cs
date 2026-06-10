#nullable disable
using AutoMapper;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO.Config;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Config;
using Master.Infrastructure.IServices.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.Services.Services.Config
{
    public class MasterErpglobalConfigService<TMasterErpglobalConfigDTO, TMasterErpGlobalConfigs, TContext> : BaseService<TMasterErpglobalConfigDTO, TMasterErpGlobalConfigs, TContext>, IMasterErpglobalConfigService<TMasterErpglobalConfigDTO>
        where TMasterErpGlobalConfigs : MasterErpGlobalConfigs, new()
        where TMasterErpglobalConfigDTO : MasterErpglobalConfigDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterErpglobalConfigRepository<TMasterErpGlobalConfigs> _repository;

        public MasterErpglobalConfigService(TContext dbContext, IMapper mapper, IMasterErpglobalConfigRepository<TMasterErpGlobalConfigs> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterErpglobalConfigDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateConfig(TMasterErpglobalConfigDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "Config created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditConfig(TMasterErpglobalConfigDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "Config updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<MasterErpglobalConfigDTO> GetFirstConfig()
        {
            var entity = await _repository.GetFirstConfig();
            return _mapper.Map<MasterErpglobalConfigDTO>(entity);
        }

        public async Task<DataSourceResult> GetAll(DataSourceRequest request)
        {
            var result = await _repository.GetAll(request);
            if (result?.Data != null)
            {
                var dtos = _mapper.Map<IEnumerable<TMasterErpglobalConfigDTO>>(
                    result.Data.Cast<TMasterErpGlobalConfigs>()
                );
                result.Data = dtos;
            }
            return result;
        }
    }
}
