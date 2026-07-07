using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.EFCore.Repositories
{
    public class FileTypeMilestonesRepository : IFileTypeMilestonesRepository
    {
        private readonly DmsReferenceContext _context;

        public FileTypeMilestonesRepository(DmsReferenceContext context)
        {
            _context = context;
        }

        public async Task<TnTypesDossier?> FindFileTypeAsync(string fileTypeCode)
        {
            var normalizedCode = fileTypeCode.Trim().ToUpper();
            return await _context.TnTypesDossiers
                .FirstOrDefaultAsync(t => t.CodeTypeDossier != null && t.CodeTypeDossier.Trim().ToUpper() == normalizedCode);
        }

        public async Task<List<FileTypeMilestoneDto>> GetMappedMilestonesAsync(TnTypesDossier fileType)
        {
            return await (
                from mapping in _context.TnFileTypeSteps.AsNoTracking()
                join step in _context.TnCodesEtapes.AsNoTracking()
                    on new { Code = mapping.StepCode, mapping.TenantId }
                    equals new { Code = step.CodeEtape, step.TenantId }
                where mapping.FileType == fileType.CodeTypeDossier
                   && mapping.TenantId == fileType.TenantId
                orderby mapping.OrdreEtape ?? step.OrdreEtape ?? 0, step.CodeEtape
                select new FileTypeMilestoneDto
                {
                    MappingId = mapping.Id,
                    FileType = mapping.FileType,
                    CodeEtape = step.CodeEtape,
                    LibelleEtape = step.LibelleEtape,
                    CategorieEtape = step.CategorieEtape,
                    OrdreEtape = mapping.OrdreEtape ?? step.OrdreEtape,
                    IsActive = step.IsActive ?? false,
                    Obligatoire = mapping.Obligatoire,
                    LimiteAvertissement = mapping.LimiteAvertissement
                })
                .ToListAsync();
        }

        public async Task<List<MilestoneStepDto>> SearchMilestonesForFileTypeAsync(TnTypesDossier fileType, string? search)
        {
            var query = _context.TnCodesEtapes
                .AsNoTracking()
                .Where(step => step.TenantId == fileType.TenantId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(step =>
                    step.CodeEtape.ToLower().Contains(term) ||
                    step.LibelleEtape.ToLower().Contains(term));
            }

            return await query
                .OrderBy(step => step.OrdreEtape ?? 0)
                .ThenBy(step => step.CodeEtape)
                .Take(200)
                .Select(step => new MilestoneStepDto
                {
                    CodeEtape = step.CodeEtape,
                    LibelleEtape = step.LibelleEtape,
                    CategorieEtape = step.CategorieEtape,
                    OrdreEtape = step.OrdreEtape,
                    IsActive = step.IsActive ?? false
                })
                .ToListAsync();
        }

        public async Task<Guid?> FindFallbackTenantAsync(string normalizedCode)
        {
            return await _context.TnFileTypeSteps
                .AsNoTracking()
                .Where(mapping => mapping.FileType != null && mapping.FileType.Trim().ToUpper() == normalizedCode)
                .Select(mapping => (Guid?)mapping.TenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TnCodesEtape>> GetStepsByCodesAsync(List<string> stepCodes, Guid tenantId)
        {
            return await _context.TnCodesEtapes
                .Where(step => stepCodes.Contains(step.CodeEtape) && step.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<List<string>> GetExistingMappingStepCodesAsync(string fileTypeCode, Guid tenantId)
        {
            return await _context.TnFileTypeSteps
                .Where(mapping => mapping.FileType == fileTypeCode && mapping.TenantId == tenantId)
                .Select(mapping => mapping.StepCode)
                .ToListAsync();
        }

        public async Task<int> GetMaxMappingOrderAsync(string fileTypeCode, Guid tenantId)
        {
            var mappings = _context.TnFileTypeSteps
                .Where(mapping => mapping.FileType == fileTypeCode && mapping.TenantId == tenantId);
            
            if (await mappings.AnyAsync())
            {
                return await mappings.MaxAsync(mapping => mapping.OrdreEtape) ?? 0;
            }
            return 0;
        }

        public async Task AddFileTypeStepsAsync(List<TnFileTypeStep> mappings)
        {
            await _context.TnFileTypeSteps.AddRangeAsync(mappings);
        }

        public async Task<int> GetMaxStepOrderAsync(Guid tenantId)
        {
            var steps = _context.TnCodesEtapes
                .Where(step => step.TenantId == tenantId);

            if (await steps.AnyAsync())
            {
                return await steps.MaxAsync(step => step.OrdreEtape) ?? 0;
            }
            return 0;
        }

        public async Task AddStepAsync(TnCodesEtape step)
        {
            await _context.TnCodesEtapes.AddAsync(step);
        }

        public async Task AddFileTypeStepAsync(TnFileTypeStep mapping)
        {
            await _context.TnFileTypeSteps.AddAsync(mapping);
        }

        public async Task<TnFileTypeStep?> GetMappingByIdAsync(int mappingId)
        {
            return await _context.TnFileTypeSteps.FindAsync(mappingId);
        }

        public async Task<bool> StepCodeExistsAsync(string code, Guid tenantId)
        {
            return await _context.TnCodesEtapes.AnyAsync(step => step.CodeEtape == code && step.TenantId == tenantId);
        }

        public void RemoveFileTypeStep(TnFileTypeStep mapping)
        {
            _context.TnFileTypeSteps.Remove(mapping);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
