using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Master.Common.Classes.Services;
namespace DMS.Application.Services
{
    public class RegimeService<TRegimeDTO, TRegime, TContext>
        : BaseService<TRegimeDTO, TRegime, TContext>, IRegimeService<TRegimeDTO>
        where TRegime : TnCodesRegime, new()
        where TRegimeDTO : RegimeDto
        where TContext : DmsReferenceContext
    {
        private readonly IRegimeRepository<TRegime> _repository;
        public RegimeService(
            TContext dbContext,
            IMapper mapper,
            IRegimeRepository<TRegime> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }
        public async Task<List<RegimeDto>> GetAllAsync()
        {
            var regimes = await _repository.GetAllAsync();
            return regimes.Select(x => new RegimeDto
            {
                CodeRegime = x.CodeRegime,
                Label = x.Label,
                DescriptionRegime = x.DescriptionRegime,
                Acronym = x.Acronym,
                IsActive = x.IsActive ?? false
            }).ToList();
        }
        public async Task<List<RegimeDto>> GetByFileTypeAsync(string codeTypeDossier)
        {
            var regimes = await _repository.GetByFileTypeAsync(codeTypeDossier);
            return regimes.Select(x => new RegimeDto
            {
                CodeRegime = x.CodeRegime,
                Label = x.Label,
                DescriptionRegime = x.DescriptionRegime,
                Acronym = x.Acronym,
                IsActive = x.IsActive ?? false
            }).ToList();
        }
        // Génère le code (GUID tronqué, comme FileType) + crée le Regime + lie auto si IsActive
        public async Task CreateAsync(CreateRegimeDto dto)
        {
            var code = Guid.NewGuid().ToString().Substring(0, 10).ToUpper();
            var newRegime = new TnCodesRegime
            {
                CodeRegime = code,
                Label = dto.Label,
                DescriptionRegime = dto.DescriptionRegime,
                Acronym = dto.Acronym,
                IsActive = dto.IsActive,
                Session = 0,
                AddNewTime = DateTime.UtcNow,
                EditTime = DateTime.UtcNow
            };
            await _repository.AddAsync(newRegime);
            if (dto.IsActive && !string.IsNullOrEmpty(dto.CodeTypeDossier))
            {
                await _repository.LinkToFileTypeAsync(dto.CodeTypeDossier, code);
            }
        }
        // Lien idempotent depuis le dropdown "Select Regimes..."
        public async Task LinkAsync(LinkRegimeDto dto)
        {
            foreach (var regimeCode in dto.RegimeCodes)
            {
                await _repository.LinkToFileTypeAsync(dto.CodeTypeDossier, regimeCode);
            }
        }
        public async Task UnlinkAsync(string codeTypeDossier, string codeRegime)
        {
            await _repository.UnlinkFromFileTypeAsync(codeTypeDossier, codeRegime);
        }
        public Task<TRegimeDTO> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}