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
    public class ERPCountryService<TErpCountryDTO, TErpCountry, TContext> : BaseService<TErpCountryDTO, TErpCountry, TContext>, IErpCountryService<TErpCountryDTO>
        where TErpCountry : ErpCountries, new()
        where TErpCountryDTO : ErpCountryDTO
        where TContext : ERPMasterContext
    {
        private readonly IErpCountryRepository<TErpCountry> _repository;

        public ERPCountryService(TContext dbContext, IMapper mapper, IErpCountryRepository<TErpCountry> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TErpCountryDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateCountry(TErpCountryDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "Country created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditCountry(TErpCountryDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "Country updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveCountry(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "Country removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllCountries(DataSourceRequest requestModel)
        {
            return await _repository.GetAllCountries(requestModel);
        }
    }
}
