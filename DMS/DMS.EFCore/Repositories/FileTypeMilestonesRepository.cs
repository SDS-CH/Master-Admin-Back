using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class FileTypeMilestonesRepository
        : GenericBaseRepository<TnCodesEtape, DmsReferenceContext>, IFileTypeMilestonesRepository<TnCodesEtape>
    {
        public FileTypeMilestonesRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<TnTypesDossier?> FindFileTypeAsync(string fileTypeCode)
        {
            var code = fileTypeCode.Trim();
            return await dbContext.TnTypesDossiers
                .FirstOrDefaultAsync(x => x.CodeTypeDossier == code);
        }

        // Projection commune : jalons mappés au type de dossier
        private IQueryable<FileTypeMilestoneDto> MappedMilestonesQuery(TnTypesDossier fileType)
        {
            return from map in dbContext.TnFileTypeSteps
                   join step in dbContext.TnCodesEtapes
                       on map.StepCode equals step.CodeEtape
                   where map.FileType == fileType.CodeTypeDossier
                         && map.TenantId == fileType.TenantId
                         && step.TenantId == fileType.TenantId
                   orderby map.OrdreEtape
                   select new FileTypeMilestoneDto
                   {
                       MappingId = map.Id,
                       FileType = map.FileType,
                       Obligatoire = map.Obligatoire,
                       GestionDelai = map.LimiteAvertissement != null,
                       LimiteAvertissement = map.LimiteAvertissement,
                       CodeEtape = step.CodeEtape,
                       LibelleEtape = step.LibelleEtape,
                       CategorieEtape = step.CategorieEtape,
                       OrdreEtape = map.OrdreEtape,
                       IsActive = step.IsActive,
                       AddNewTime = step.AddNewTime,
                       EditTime = step.EditTime
                   };
        }

        public async Task<DataSourceResult> GetAllMappedMilestones(DataSourceRequest requestModel, TnTypesDossier fileType)
        {
            return await MappedMilestonesQuery(fileType).ToDataSourceResultAsync(requestModel);
        }

        public async Task<List<FileTypeMilestoneDto>> GetMappedMilestonesAsync(TnTypesDossier fileType)
        {
            return await MappedMilestonesQuery(fileType).ToListAsync();
        }

        // Jalons du tenant NON encore mappés à ce type de dossier (candidats à ajouter)
        private IQueryable<TnCodesEtape> AvailableStepsQuery(TnTypesDossier fileType)
        {
            var mappedCodes = dbContext.TnFileTypeSteps
                .Where(m => m.FileType == fileType.CodeTypeDossier && m.TenantId == fileType.TenantId)
                .Select(m => m.StepCode);

            return dbContext.TnCodesEtapes
                .Where(s => s.TenantId == fileType.TenantId && !mappedCodes.Contains(s.CodeEtape));
        }

        private static MilestoneStepDto ToStepDto(TnCodesEtape s) => new MilestoneStepDto
        {
            CodeEtape = s.CodeEtape,
            LibelleEtape = s.LibelleEtape,
            CategorieEtape = s.CategorieEtape,
            OrdreEtape = s.OrdreEtape,
            IsActive = s.IsActive,
            AddNewTime = s.AddNewTime,
            EditTime = s.EditTime
        };

        public async Task<DataSourceResult> SearchMilestonesForFileType(DataSourceRequest requestModel, TnTypesDossier fileType, MilestoneStepDto filter)
        {
            var query = AvailableStepsQuery(fileType);

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.LibelleEtape))
                    query = query.Where(s => s.LibelleEtape.Contains(filter.LibelleEtape));

                if (!string.IsNullOrWhiteSpace(filter.CategorieEtape))
                    query = query.Where(s => s.CategorieEtape == filter.CategorieEtape);

                if (!string.IsNullOrWhiteSpace(filter.CodeEtape))
                    query = query.Where(s => s.CodeEtape.Contains(filter.CodeEtape));
            }

            return await query
                .OrderBy(s => s.LibelleEtape)
                .Select(s => new MilestoneStepDto
                {
                    CodeEtape = s.CodeEtape,
                    LibelleEtape = s.LibelleEtape,
                    CategorieEtape = s.CategorieEtape,
                    OrdreEtape = s.OrdreEtape,
                    IsActive = s.IsActive,
                    AddNewTime = s.AddNewTime,
                    EditTime = s.EditTime
                })
                .ToDataSourceResultAsync(requestModel);
        }

        public async Task<List<MilestoneStepDto>> SearchMilestonesForFileTypeAsync(TnTypesDossier fileType, string? search)
        {
            var query = AvailableStepsQuery(fileType);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(s => s.LibelleEtape.Contains(term) || s.CodeEtape.Contains(term));
            }

            var steps = await query.OrderBy(s => s.LibelleEtape).ToListAsync();
            return steps.Select(ToStepDto).ToList();
        }

        public async Task<Guid?> FindFallbackTenantAsync(string normalizedCode)
        {
            // Le type de dossier n'existe pas comme ligne : on récupère un tenant de référence
            var tenant = await dbContext.TnCodesEtapes
                .Select(s => (Guid?)s.TenantId)
                .FirstOrDefaultAsync();

            if (tenant != null) return tenant;

            return await dbContext.TnTypesDossiers
                .Select(t => (Guid?)t.TenantId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TnCodesEtape>> GetStepsByCodesAsync(List<string> stepCodes, Guid tenantId)
        {
            return await dbContext.TnCodesEtapes
                .Where(s => s.TenantId == tenantId && stepCodes.Contains(s.CodeEtape))
                .ToListAsync();
        }

        public async Task<List<string>> GetExistingMappingStepCodesAsync(string fileTypeCode, Guid tenantId)
        {
            return await dbContext.TnFileTypeSteps
                .Where(m => m.FileType == fileTypeCode && m.TenantId == tenantId)
                .Select(m => m.StepCode)
                .ToListAsync();
        }

        public async Task<int> GetMaxMappingOrderAsync(string fileTypeCode, Guid tenantId)
        {
            return (await dbContext.TnFileTypeSteps
                .Where(m => m.FileType == fileTypeCode && m.TenantId == tenantId)
                .MaxAsync(m => (int?)m.OrdreEtape)) ?? 0;
        }

        public Task AddFileTypeStepsAsync(List<TnFileTypeStep> mappings)
        {
            return dbContext.TnFileTypeSteps.AddRangeAsync(mappings);
        }

        public async Task<int> GetMaxStepOrderAsync(Guid tenantId)
        {
            return (await dbContext.TnCodesEtapes
                .Where(s => s.TenantId == tenantId)
                .MaxAsync(s => (int?)s.OrdreEtape)) ?? 0;
        }

        public async Task AddStepAsync(TnCodesEtape step)
        {
            await dbContext.TnCodesEtapes.AddAsync(step);
        }

        public async Task AddFileTypeStepAsync(TnFileTypeStep mapping)
        {
            await dbContext.TnFileTypeSteps.AddAsync(mapping);
        }

        public async Task<TnFileTypeStep?> GetMappingByIdAsync(int mappingId)
        {
            return await dbContext.TnFileTypeSteps
                .FirstOrDefaultAsync(m => m.Id == mappingId);
        }

        public async Task<bool> StepCodeExistsAsync(string code, Guid tenantId)
        {
            return await dbContext.TnCodesEtapes
                .AnyAsync(s => s.CodeEtape == code && s.TenantId == tenantId);
        }

        public async Task<TnCodesEtape?> GetStepByLabelAsync(string libelle, Guid tenantId)
        {
            return await dbContext.TnCodesEtapes
                .FirstOrDefaultAsync(s => s.LibelleEtape == libelle && s.TenantId == tenantId);
        }

        public void RemoveFileTypeStep(TnFileTypeStep mapping)
        {
            dbContext.TnFileTypeSteps.Remove(mapping);
        }

        // Commit unique : positionne le tenant_id PostgreSQL (RLS) dans la même transaction
        public async Task SaveChangesAsync()
        {
            var tenantId = ResolveTrackedTenant();

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                if (tenantId != Guid.Empty)
                {
                    await dbContext.Database.ExecuteSqlRawAsync(
                        "SELECT set_config('app.tenant_id', {0}, true)", tenantId.ToString());
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Récupère le tenant des entités suivies par le ChangeTracker
        private Guid ResolveTrackedTenant()
        {
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                switch (entry.Entity)
                {
                    case TnFileTypeStep step when step.TenantId != Guid.Empty:
                        return step.TenantId;
                    case TnCodesEtape codeEtape when codeEtape.TenantId != Guid.Empty:
                        return codeEtape.TenantId;
                }
            }

            return Guid.Empty;
        }
    }
}
