using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Master.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class FileTypeMilestonesService : IFileTypeMilestonesService
    {
        private readonly IFileTypeMilestonesRepository _repository;

        public FileTypeMilestonesService(IFileTypeMilestonesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FileTypeMilestoneDto>?> GetMappedMilestonesAsync(string fileTypeCode)
        {
            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
            if (fileType is null) return null;

            return await _repository.GetMappedMilestonesAsync(fileType);
        }

        public async Task<List<MilestoneStepDto>?> SearchMilestonesAsync(string fileTypeCode, string? search)
        {
            var normalizedCode = fileTypeCode.Trim().ToUpper();
            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);

            if (fileType is null)
            {
                var fallbackTenant = await _repository.FindFallbackTenantAsync(normalizedCode);
                if (fallbackTenant is null)
                {
                    return null;
                }

                fileType = new TnTypesDossier
                {
                    CodeTypeDossier = fileTypeCode.Trim(),
                    TenantId = fallbackTenant.Value
                };
            }

            return await _repository.SearchMilestonesForFileTypeAsync(fileType, search);
        }
        public async Task<OperationResult> CreateStep(AddFileTypeMilestonesDto stepsEditorDTO)
        {
            try
            {
                Guid g = Guid.NewGuid();
                string code = String.Format(g.ToString().Replace("-", "").Substring(0, 8));
                TnCodesEtapes existingStep = await _stepsCodesRepository.GenericGetFirstOrDefaultAsync(e => e.LibelleEtape == stepsEditorDTO.LibelleEtape);
                if (existingStep != null)
                {
                    return new OperationResult { ErrorOccured = true, Message = String.Format("StepAlreadyExists", String.Join(", ", stepsEditorDTO.LibelleEtape)) };
                }
                TnCodesEtapes newStep = new TnCodesEtapes();
                stepsEditorDTO.CodeEtape = code;
                stepsEditorDTO.Session = 0;
                stepsEditorDTO.AddNewTime = _stepsCodesRepository.ToUnspecified(DateTime.Now);
                stepsEditorDTO.EditTime = _stepsCodesRepository.ToUnspecified(DateTime.Now);
                await _stepsCodesRepository.Create(stepsEditorDTO.MapTo<TnCodesEtapes>(_mapper));
                return new OperationResult(false, _translate["OperationTerminatedSuccessfully"].Value, code);
            }
            catch (Exception e)
            {
                return new OperationResult(true, e.Message);
            }
        }

        public async Task<string> AddMilestonesAsync(string fileTypeCode, AddFileTypeMilestonesDto dto)
        {
            if (dto.StepCodes.Count == 0)
            {
                return "Select at least one milestone.";
            }

            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
            if (fileType is null)
            {
                return "File type not found.";
            }

            var stepCodes = dto.StepCodes
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => code.Trim())
                .Distinct()
                .ToList();

            var steps = await _repository.GetStepsByCodesAsync(stepCodes, fileType.TenantId);
            if (steps.Count == 0)
            {
                return "No selected milestone exists for this file type tenant.";
            }

            var existingCodes = await _repository.GetExistingMappingStepCodesAsync(fileType.CodeTypeDossier, fileType.TenantId);
            var existingSet = existingCodes.ToHashSet();
            var nextOrder = await _repository.GetMaxMappingOrderAsync(fileType.CodeTypeDossier, fileType.TenantId);

            var newMappings = new List<TnFileTypeStep>();
            foreach (var step in steps)
            {
                if (existingSet.Contains(step.CodeEtape))
                {
                    continue;
                }

                nextOrder++;
                newMappings.Add(new TnFileTypeStep
                {
                    FileType = fileType.CodeTypeDossier,
                    StepCode = step.CodeEtape,
                    Obligatoire = false,
                    LimiteAvertissement = null,
                    OrdreEtape = nextOrder,
                    DateAchvementObligatoire = false,
                    ObligatoireSiRegime = null,
                    IsFileCover = false,
                    TenantId = fileType.TenantId
                });
            }

            if (newMappings.Count == 0)
            {
                return "Selected milestones are already mapped.";
            }

            await _repository.AddFileTypeStepsAsync(newMappings);
            await _repository.SaveChangesAsync();
            return string.Empty; // Success
        }

        public async Task<MilestoneStepDto> CreateStepAsync(string fileTypeCode, CreateMilestoneStepDto dto)
        {
            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
            if (fileType is null)
            {
                throw new Exception("File type not found.");
            }

            var code = await GenerateStepCode(dto.LibelleEtape, fileType.TenantId);
            var nextStepOrder = await _repository.GetMaxStepOrderAsync(fileType.TenantId) + 1;

            var step = new TnCodesEtape
            {
                CodeEtape = code,
                LibelleEtape = dto.LibelleEtape.Trim(),
                CategorieEtape = string.IsNullOrWhiteSpace(dto.CategorieEtape) ? null : dto.CategorieEtape.Trim(),
                OrdreEtape = nextStepOrder,
                GestionDuree = false,
                DelaiEtape = null,
                Agence = null,
                EtapeDossier = true,
                Regime = null,
                PlanAchatEtape = null,
                Session = 0,
                AddNewTime = DateTime.UtcNow,
                EditTime = DateTime.UtcNow,
                AttachedDocumentType = null,
                TypesDocuments = null,
                IsActive = dto.IsActive,
                TenantId = fileType.TenantId
            };

            await _repository.AddStepAsync(step);

            if (dto.IsActive)
            {
                var nextMappingOrder = await _repository.GetMaxMappingOrderAsync(fileType.CodeTypeDossier, fileType.TenantId) + 1;
                await _repository.AddFileTypeStepAsync(new TnFileTypeStep
                {
                    FileType = fileType.CodeTypeDossier,
                    StepCode = step.CodeEtape,
                    Obligatoire = false,
                    LimiteAvertissement = null,
                    OrdreEtape = nextMappingOrder,
                    DateAchvementObligatoire = false,
                    ObligatoireSiRegime = null,
                    IsFileCover = false,
                    TenantId = fileType.TenantId
                });
            }

            await _repository.SaveChangesAsync();

            return new MilestoneStepDto
            {
                CodeEtape = step.CodeEtape,
                LibelleEtape = step.LibelleEtape,
                CategorieEtape = step.CategorieEtape,
                OrdreEtape = step.OrdreEtape,
                IsActive = step.IsActive ?? false
            };
        }

        public async Task<bool> UpdateMilestoneMappingAsync(int mappingId, UpdateFileTypeMilestoneDto dto)
        {
            var mapping = await _repository.GetMappingByIdAsync(mappingId);
            if (mapping is null) return false;

            mapping.Obligatoire = dto.Obligatoire;
            mapping.LimiteAvertissement = dto.LimiteAvertissement;

            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMilestoneMappingAsync(int mappingId)
        {
            var mapping = await _repository.GetMappingByIdAsync(mappingId);
            if (mapping is null) return false;

            _repository.RemoveFileTypeStep(mapping);
            await _repository.SaveChangesAsync();
            return true;
        }

        private async Task<string> GenerateStepCode(string label, Guid tenantId)
        {
            var words = label
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(word => char.ToUpperInvariant(word[0]))
                .ToArray();

            var baseCode = new string(words);
            if (string.IsNullOrWhiteSpace(baseCode))
            {
                baseCode = "STEP";
            }

            baseCode = KeepCodeCharacters(baseCode);
            if (baseCode.Length > 7)
            {
                baseCode = baseCode[..7];
            }

            var code = baseCode;
            var suffix = 1;

            while (await _repository.StepCodeExistsAsync(code, tenantId))
            {
                var suffixText = suffix.ToString();
                var prefixLength = Math.Min(baseCode.Length, 10 - suffixText.Length);
                code = $"{baseCode[..prefixLength]}{suffixText}";
                suffix++;
            }

            return code;
        }

        private static string KeepCodeCharacters(string value)
        {
            var builder = new StringBuilder();
            foreach (var character in value)
            {
                if (char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                }
            }
            return builder.Length == 0 ? "STEP" : builder.ToString();
        }
    }
}
