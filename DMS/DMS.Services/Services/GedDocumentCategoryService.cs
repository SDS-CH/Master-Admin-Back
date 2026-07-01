using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Master.Common.Classes.Services;
using Microsoft.EntityFrameworkCore;

namespace DMS.Services.Services
{
    public class GedDocumentCategoryService<TCategoryDTO, TCategory, TContext>
        : BaseService<TCategoryDTO, TCategory, TContext>, IGedDocumentCategoryService<TCategoryDTO>
        where TCategory : GedDocumentCategory, new()
        where TCategoryDTO : GedDocumentCategoryDto
        where TContext : DmsReferenceContext
    {
        private readonly IGedDocumentCategoryRepository<TCategory> _repository;

        public GedDocumentCategoryService(TContext dbContext, IMapper mapper, IGedDocumentCategoryRepository<TCategory> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TCategoryDTO>> GetCategories()
        {
            var categories = await _repository.GetCategories();
            return _mapper.Map<IEnumerable<TCategoryDTO>>(categories);
        }

        public Task<IEnumerable<GedDocumentTypeDto>> GetTypes(int categoryId, int? industryId, bool unassignedOnly = false)
        {
            return _repository.GetTypes(categoryId, industryId, unassignedOnly);
        }

        public async Task<(bool Success, string? Error, GedDocumentTypeDto? Data)> CreateType(GedDocumentTypeDto dto)
        {
            NormalizeTypeDto(dto);

            if (string.IsNullOrWhiteSpace(dto.TypeName))
            {
                return (false, "TypeName is required.", null);
            }

            try
            {
                dto.CodeTemplate = await GenerateUniqueDocumentCode(dto);
                var result = await _repository.CreateType(dto);
                return (true, null, result);
            }
            catch (DbUpdateException ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Success, string? Error, GedDocumentTypeDto? Data)> UpdateType(int id, GedDocumentTypeDto dto)
        {
            NormalizeTypeDto(dto);

            if (string.IsNullOrWhiteSpace(dto.TypeName))
            {
                return (false, "TypeName is required.", null);
            }

            try
            {
                var result = await _repository.UpdateType(id, dto);
                return result is null
                    ? (false, "Document type not found.", null)
                    : (true, null, result);
            }
            catch (DbUpdateException ex)
            {
                return (false, ex.InnerException?.Message ?? ex.Message, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public Task<bool> DeleteType(int id)
        {
            return _repository.DeleteType(id);
        }

        private static void NormalizeTypeDto(GedDocumentTypeDto dto)
        {
            dto.TypeName = dto.TypeName?.Trim();
            dto.TypeNameEn = string.IsNullOrWhiteSpace(dto.TypeNameEn) ? dto.TypeName : dto.TypeNameEn.Trim();
            dto.UrlTemplate = dto.UrlTemplate ?? string.Empty;
            dto.TypeNameFr = EmptyAsNull(dto.TypeNameFr);
            dto.TypeNameEs = EmptyAsNull(dto.TypeNameEs);
            dto.TypeNamePt = EmptyAsNull(dto.TypeNamePt);
            dto.CodeTemplate = EmptyAsNull(dto.CodeTemplate);
            dto.DefaultFolder = EmptyAsNull(dto.DefaultFolder);
            dto.TypeRole = EmptyAsNull(dto.TypeRole);
        }

        private static string? EmptyAsNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private async Task<string> GenerateUniqueDocumentCode(GedDocumentTypeDto dto)
        {
            var existingTypes = await _repository.GetTypes(dto.TypeCategory, dto.IndustryId, dto.IndustryId == null);
            var existingCodes = existingTypes
                .Select(type => type.CodeTemplate?.Trim().ToUpperInvariant())
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .ToHashSet();

            var baseCode = NormalizeCode(dto.CodeTemplate);
            var candidate = baseCode;
            var suffix = 2;

            while (existingCodes.Contains(candidate))
            {
                var suffixText = suffix.ToString();
                var maxBaseLength = Math.Max(1, 20 - suffixText.Length);
                candidate = $"{baseCode[..Math.Min(baseCode.Length, maxBaseLength)]}{suffixText}";
                suffix++;
            }

            return candidate;
        }

        private static string NormalizeCode(string? code)
        {
            var normalized = string.IsNullOrWhiteSpace(code) ? "DOC" : code.Trim().ToUpperInvariant();
            normalized = new string(normalized
                .Where(ch => char.IsLetterOrDigit(ch) || ch == '_')
                .ToArray());

            return string.IsNullOrWhiteSpace(normalized) ? "DOC" : normalized;
        }
    }
}
