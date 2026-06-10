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
    public class CurrencyService<TCurrencyDTO, TCurrency, TContext> : BaseService<TCurrencyDTO, TCurrency, TContext>, ICurrencyService<TCurrencyDTO>
        where TCurrency : Currency, new()
        where TCurrencyDTO : CurrencyDTO
        where TContext : ERPMasterContext
    {
        private readonly ICurrencyRepository<TCurrency> _repository;

        public CurrencyService(TContext dbContext, IMapper mapper, ICurrencyRepository<TCurrency> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TCurrencyDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateCurrency(TCurrencyDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "Currency created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditCurrency(TCurrencyDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "Currency updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveCurrency(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "Currency removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllCurrencies(DataSourceRequest requestModel)
        {
            return await _repository.GetAllCurrencies(requestModel);
        }
    }
}
