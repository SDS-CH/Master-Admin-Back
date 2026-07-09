using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class FileTypeMilestonesService<TEntityDTO, TEntity, TContext>
        : BaseService<TEntityDTO, TEntity, TContext>, IFileTypeMilestonesService<TEntityDTO>
        where TEntity : TnCodesEtape, new()
        where TEntityDTO : MilestoneStepDto
        where TContext : DmsReferenceContext
    {
        private readonly IFileTypeMilestonesRepository<TEntity> _repository;

        public FileTypeMilestonesService(TContext dbContext, IMapper mapper, IFileTypeMilestonesRepository<TEntity> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public Task<TEntityDTO> GetById(int id)
        {
            return _repository.GetById(id)
                .ContinueWith(task => _mapper.Map<TEntityDTO>(task.Result));
        }

        // --- Nouvelle méthode : pagination/tri/filtrage Kendo pour les jalons mappés ---
        public async Task<DataSourceResult?> GetAllMappedMilestonesAsync(string fileTypeCode, DataSourceRequest requestModel)
        {
            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
            if (fileType is null) return null;

            return await _repository.GetAllMappedMilestones(requestModel, fileType);
        }

        public async Task<List<FileTypeMilestoneDto>?> GetMappedMilestonesAsync(string fileTypeCode)
        {
            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
            if (fileType is null) return null;

            return await _repository.GetMappedMilestonesAsync(fileType);
        }

        // --- Nouvelle méthode : pagination/tri/filtrage Kendo pour la recherche de jalons ---
        public async Task<DataSourceResult?> SearchMilestonesAsync(string fileTypeCode, DataSourceRequest requestModel, MilestoneStepDto filter)
        {
            var fileType = await ResolveOrBuildFallbackFileTypeAsync(fileTypeCode);
            if (fileType is null) return null;

            return await _repository.SearchMilestonesForFileType(requestModel, fileType, filter);
        }

        public async Task<List<MilestoneStepDto>?> SearchMilestonesAsync(string fileTypeCode, string? search)
        {
            var fileType = await ResolveOrBuildFallbackFileTypeAsync(fileTypeCode);
            if (fileType is null) return null;

            return await _repository.SearchMilestonesForFileTypeAsync(fileType, search);
        }

        public async Task<OperationResult> AddMilestonesAsync(string fileTypeCode, AddFileTypeMilestonesDto dto)
        {
            try
            {
                if (dto.StepCodes.Count == 0)
                {
                    return new OperationResult(true, "Select at least one milestone.");
                }

                var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
                if (fileType is null)
                {
                    return new OperationResult(true, "File type not found.");
                }

                var stepCodes = dto.StepCodes
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Select(code => code.Trim())
                    .Distinct()
                    .ToList();

                var steps = await _repository.GetStepsByCodesAsync(stepCodes, fileType.TenantId);
                if (steps.Count == 0)
                {
                    return new OperationResult(true, "No selected milestone exists for this file type tenant.");
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
                    // No-op bénin : les jalons sélectionnés sont déjà mappés → succès (HTTP 200)
                    return new OperationResult(false, "Selected milestones are already mapped.");
                }

                await _repository.AddFileTypeStepsAsync(newMappings);
                await _repository.SaveChangesAsync();
                return new OperationResult(false, "Operation terminated successfully.");
            }
            catch (Exception e)
            {
                return new OperationResult(true, e.Message);
            }
        }

        public async Task<OperationResult> CreateStepAsync(string fileTypeCode, CreateMilestoneStepDto dto)
        {
            try
            {
                var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
                if (fileType is null)
                {
                    return new OperationResult(true, "File type not found.");
                }

                // Vérification de doublon sur le libellé, comme dans le pattern original
                var existingStep = await _repository.GetStepByLabelAsync(dto.LibelleEtape.Trim(), fileType.TenantId);
                if (existingStep != null)
                {
                    return new OperationResult(true, $"A step already exists with the label '{dto.LibelleEtape}'.");
                }

                var code = await GenerateStepCode(dto.LibelleEtape, fileType.TenantId);
                var nextStepOrder = await _repository.GetMaxStepOrderAsync(fileType.TenantId) + 1;
                var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

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
                    AddNewTime = now,
                    EditTime = now,
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

                return new OperationResult(false, "Operation terminated successfully.", step.CodeEtape);
            }
            catch (Exception e)
            {
                return new OperationResult(true, e.Message);
            }
        }

        public async Task<OperationResult> UpdateMilestoneMappingAsync(int mappingId, UpdateFileTypeMilestoneDto dto)
        {
            try
            {
                var mapping = await _repository.GetMappingByIdAsync(mappingId);
                if (mapping is null)
                {
                    return new OperationResult(true, "Milestone mapping not found.");
                }

                mapping.Obligatoire = dto.Obligatoire;
                mapping.LimiteAvertissement = dto.LimiteAvertissement;

                await _repository.SaveChangesAsync();
                return new OperationResult(false, "Operation terminated successfully.");
            }
            catch (Exception e)
            {
                return new OperationResult(true, e.Message);
            }
        }

        public async Task<OperationResult> DeleteMilestoneMappingAsync(int mappingId)
        {
            try
            {
                var mapping = await _repository.GetMappingByIdAsync(mappingId);
                if (mapping is null)
                {
                    return new OperationResult(true, "Milestone mapping delete error.");
                }

                _repository.RemoveFileTypeStep(mapping);
                await _repository.SaveChangesAsync();
                return new OperationResult(false, "Operation terminated successfully.");
            }
            catch (Exception)
            {
                return new OperationResult(true, "Milestone mapping delete error.");
            }
        }

        // --- Factorisation : logique de résolution du type de dossier (avec fallback tenant) ---
        private async Task<TnTypesDossier?> ResolveOrBuildFallbackFileTypeAsync(string fileTypeCode)
        {
            var normalizedCode = fileTypeCode.Trim().ToUpper();
            var fileType = await _repository.FindFileTypeAsync(fileTypeCode);

            if (fileType is not null) return fileType;

            var fallbackTenant = await _repository.FindFallbackTenantAsync(normalizedCode);
            if (fallbackTenant is null) return null;

            return new TnTypesDossier
            {
                CodeTypeDossier = fileTypeCode.Trim(),
                TenantId = fallbackTenant.Value
            };
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