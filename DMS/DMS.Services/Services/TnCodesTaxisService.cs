#nullable disable
using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using System;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class TnCodesTaxisService<TTnCodesTaxisDTO, TTnCodesTaxis, TContext>
        : BaseService<TTnCodesTaxisDTO, TTnCodesTaxis, TContext>, ITnCodesTaxisService<TTnCodesTaxisDTO>
        where TTnCodesTaxis : TnCodesTaxis, new()
        where TTnCodesTaxisDTO : TnCodesTaxisDTO
        where TContext : DmsReferenceContext
    {
        private readonly ITnCodesTaxisRepository<TTnCodesTaxis> _repository;

        public TnCodesTaxisService(TContext dbContext, IMapper mapper, ITnCodesTaxisRepository<TTnCodesTaxis> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<DataSourceResult> GetAllTnCodesTaxis(DataSourceRequest requestModel, string countryCode = null)
        {
            return await _repository.GetAllTnCodesTaxis(requestModel, countryCode);
        }

        public async Task<TTnCodesTaxisDTO> GetById(string code)
        {
            var entity = await _repository.GetById(code);
            return _mapper.Map<TTnCodesTaxisDTO>(entity);
        }

        public Task<TTnCodesTaxisDTO> GetById(int id)
        {
            throw new NotImplementedException("TnCodesTaxis uses a string primary key.");
        }

        public async Task<OperationResult> CreateTnCodesTaxis(TTnCodesTaxisDTO entity)
        {
            try
            {
                var model = _mapper.Map<TTnCodesTaxis>(entity);
                model.AddNewTime = DateTime.UtcNow;
                model.EditTime = DateTime.UtcNow;
                await _repository.Create(model);
                return new OperationResult(false, "TnCodesTaxis created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditTnCodesTaxis(TTnCodesTaxisDTO entity, string code)
        {
            try
            {
                var existing = await _repository.GetById(code);
                if (existing == null)
                    return new OperationResult(true, "Entity not found.");
                
                _mapper.Map(entity, existing);
                existing.EditTime = DateTime.UtcNow;
                await _repository.Edit(existing);
                return new OperationResult(false, "TnCodesTaxis updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveTnCodesTaxis(string code)
        {
            try
            {
                await _repository.Delete(code);
                return new OperationResult(false, "TnCodesTaxis removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }
    }
}
