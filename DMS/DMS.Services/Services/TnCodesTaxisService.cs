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
using System.Globalization;
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

        public async Task<System.Collections.Generic.List<CompteComptableOptionDTO>> GetComptesByCountry(int countryId)
        {
            return await _repository.GetComptesByCountry(countryId);
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

                // Auto-generate the tax code: <CountryCode 2 chars>_VAT<rate>  (e.g. CH_VAT20)
                model.CodeTaxe = BuildCodeTaxe(entity.CountryCode, entity.PourcentageTaxe);

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

        /// <summary>
        /// Builds the tax code as: 2-letter country code + "_VAT" + rate. e.g. "CH" + 20 => "CH_VAT20".
        /// </summary>
        private static string BuildCodeTaxe(string countryCode, double rate)
        {
            var prefix = (countryCode ?? string.Empty).Trim().ToUpperInvariant();
            if (prefix.Length > 2) prefix = prefix.Substring(0, 2);
            if (string.IsNullOrEmpty(prefix)) prefix = "XX";

            var rateStr = rate.ToString("0.##", CultureInfo.InvariantCulture);
            return $"{prefix}_VAT{rateStr}";
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
