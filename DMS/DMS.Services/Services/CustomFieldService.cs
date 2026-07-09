using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Master.Common.Classes.Services;

namespace DMS.Application.Services
{
    public class CustomFieldService<TCustomFieldDTO, TCustomField, TContext>
        : BaseService<TCustomFieldDTO, TCustomField, TContext>, ICustomFieldService<TCustomFieldDTO>
        where TCustomField : TnCodesComplementsDossier, new()
        where TCustomFieldDTO : CustomFieldDto
        where TContext : DmsReferenceContext
    {
        private readonly ICustomFieldRepository<TCustomField> _repository;

        public CustomFieldService(
            TContext dbContext,
            IMapper mapper,
            ICustomFieldRepository<TCustomField> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<List<CustomFieldDto>> GetAllAsync()
        {
            var customFields = await _repository.GetAllAsync();
            return customFields.Select(x => new CustomFieldDto
            {
                CodeComplement = x.CodeComplement,
                Name = x.Name,
                DataType = x.DataType,
                IsRequiredOnFileClosure = false
            }).ToList();
        }

        public async Task<List<CustomFieldDto>> GetByFileTypeAsync(string codeTypeDossier)
        {
            var results = await _repository.GetByFileTypeAsync(codeTypeDossier);
            return results.Select(r => new CustomFieldDto
            {
                CodeComplement = r.CustomField.CodeComplement,
                Name = r.CustomField.Name,
                DataType = r.CustomField.DataType,
                IsRequiredOnFileClosure = r.IsRequiredOnFileClosure
            }).ToList();
        }

        public List<string> GetAvailableTypes()
        {
            return new List<string>
            {
                PV.FREETEXT_,
                PV.BIT,
                PV.DOUBLE,
                PV.INTEGER,
                PV.INT,
                PV.DATETIME,
                PV.DATE,
                PV.MASK,
                PV.SELECTLIST
            };
        }

        public async Task CreateAsync(CreateCustomFieldDto dto)
        {
            var code = GenerateCodeFromName(dto.Name);

            var newCustomField = new TnCodesComplementsDossier
            {
                CodeComplement = code,
                Name = dto.Name,
                DataType = dto.DataType,
                IsActive = true,
                Session = 0,
                AddNewTime = DateTime.UtcNow,
                EditTime = DateTime.UtcNow
            };

            await _repository.AddAsync(newCustomField);

            if (!string.IsNullOrEmpty(dto.CodeTypeDossier))
            {
                await _repository.LinkToFileTypeAsync(dto.CodeTypeDossier, code, dto.IsRequiredOnFileClosure);
            }
        }

        // Lien idempotent — chaque item a son propre IsRequiredOnFileClosure
        public async Task LinkAsync(LinkCustomFieldDto dto)
        {
            foreach (var item in dto.Items)
            {
                await _repository.LinkToFileTypeAsync(dto.CodeTypeDossier, item.ComplementCode, item.IsRequiredOnFileClosure);
            }
        }

        public async Task UnlinkAsync(string codeTypeDossier, string complementCode)
        {
            await _repository.UnlinkFromFileTypeAsync(codeTypeDossier, complementCode);
        }

        public async Task<bool> UpdateIsRequiredAsync(string codeTypeDossier, string complementCode, bool isRequiredOnFileClosure)
        {
            return await _repository.UpdateIsRequiredAsync(codeTypeDossier, complementCode, isRequiredOnFileClosure);
        }

        private static string GenerateCodeFromName(string name)
        {
            var normalized = name.Trim().ToUpperInvariant().Replace(" ", "_");
            return $"CF_{normalized}";
        }

        public Task<TCustomFieldDTO> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}