using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class RegimeService : IRegimeService
    {
        private readonly IRegimeRepository _repository;

        public RegimeService(IRegimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RegimeDto>> GetRegimesByFileTypeAsync(string fileTypeCode)
        {
            var regimes = await _repository.GetRegimesByFileTypeAsync(fileTypeCode);
            return regimes.Select(MapToDto).ToList();
        }

        public async Task<List<RegimeDto>> GetAvailableRegimesAsync(string fileTypeCode)
        {
            var regimes = await _repository.GetAvailableRegimesAsync(fileTypeCode);
            return regimes.Select(MapToDto).ToList();
        }

        public async Task<RegimeDto> CreateRegimeAndLinkAsync(CreateRegimeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CodeRegime))
                throw new InvalidOperationException("Le code du régime est requis.");

            var exists = await _repository.RegimeCodeExistsAsync(dto.CodeRegime);
            if (exists)
                throw new InvalidOperationException($"Le code régime \"{dto.CodeRegime}\" existe déjà.");

            var newRegime = new TnCodesRegime
            {
                CodeRegime = dto.CodeRegime,
                Label = dto.Label,
                DescriptionRegime = dto.DescriptionRegime,
                Acronym = dto.Acronym
            };

            var created = await _repository.CreateRegimeAsync(newRegime);

            // Lien automatique avec le File Type d'origine
            await _repository.LinkRegimeToFileTypeAsync(dto.FileTypeCode, created.CodeRegime);

            return MapToDto(created);
        }

        public async Task<RegimeDto?> UpdateRegimeAsync(string regimeCode, UpdateRegimeDto dto)
        {
            var updated = await _repository.UpdateRegimeAsync(regimeCode, dto.Label, dto.DescriptionRegime, dto.Acronym);
            return updated == null ? null : MapToDto(updated);
        }

        public async Task LinkRegimeAsync(LinkRegimeDto dto)
        {
            var alreadyLinked = await _repository.IsRegimeLinkedToFileTypeAsync(dto.FileTypeCode, dto.RegimeCode);
            if (alreadyLinked)
                throw new InvalidOperationException("Ce régime est déjà lié à ce type de dossier.");

            await _repository.LinkRegimeToFileTypeAsync(dto.FileTypeCode, dto.RegimeCode);
        }

        public async Task<bool> UnlinkRegimeAsync(string fileTypeCode, string regimeCode)
        {
            return await _repository.UnlinkRegimeFromFileTypeAsync(fileTypeCode, regimeCode);
        }

        private static RegimeDto MapToDto(TnCodesRegime r) => new RegimeDto
        {
            CodeRegime = r.CodeRegime,
            Label = r.Label,
            DescriptionRegime = r.DescriptionRegime,
            Acronym = r.Acronym
        };
    }
}
