using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class FileTypeDocumentTypeRepository
        : GenericBaseRepository<GedDocumentType, DmsReferenceContext>, IFileTypeDocumentTypeRepository<GedDocumentType>
    {
        public FileTypeDocumentTypeRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<TnTypesDossier?> FindFileTypeAsync(string fileTypeCode)
        {
            var code = fileTypeCode.Trim();
            return await dbContext.TnTypesDossiers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CodeTypeDossier == code);
        }

        public async Task<Guid?> FindFallbackTenantAsync()
        {
            var tenant = await dbContext.GedDocumentTypes
                .Select(t => (Guid?)t.TenantId)
                .FirstOrDefaultAsync();

            if (tenant != null) return tenant;

            return await dbContext.TnTypesDossiers
                .Select(t => (Guid?)t.TenantId)
                .FirstOrDefaultAsync();
        }

        // Ids des types déjà rattachés au type de dossier
        private IQueryable<int> AttachedTypeIdsQuery(string fileTypeCode)
        {
            return from map in dbContext.GedCategorieTypeTypesDossiers
                   where map.TypeDossierCode == fileTypeCode
                   join ct in dbContext.GedCategorieTypes on map.CategorieTypeId equals ct.Id
                   select ct.TypesId;
        }

        public async Task<List<GedDocumentTypeDto>> GetAttachedTypesAsync(string fileTypeCode)
        {
            var code = fileTypeCode.Trim();
            var attachedIds = AttachedTypeIdsQuery(code).Distinct();

            return await dbContext.GedDocumentTypes
                .AsNoTracking()
                .Where(t => attachedIds.Contains(t.Id))
                .OrderBy(t => t.TypeName)
                .Select(ProjectDto)
                .ToListAsync();
        }

        public async Task<DataSourceResult> SearchAvailableTypesAsync(string fileTypeCode, DataSourceRequest request)
        {
            var code = fileTypeCode.Trim();
            var attachedIds = AttachedTypeIdsQuery(code);

            // Projection SQL-traductible : Kendo applique filtre/tri/pagination côté serveur.
            var query = dbContext.GedDocumentTypes
                .AsNoTracking()
                .Where(t => !attachedIds.Contains(t.Id))
                .Select(ProjectDto);

            return await query.ToDataSourceResultAsync(request);
        }

        public async Task<List<GedDocumentType>> GetTypesByIdsAsync(List<int> typeIds)
        {
            return await dbContext.GedDocumentTypes
                .Where(t => typeIds.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<GedCategorieType?> FindCategorieTypeAsync(int typeId, int categorieId)
        {
            return await dbContext.GedCategorieTypes
                .FirstOrDefaultAsync(x => x.TypesId == typeId && x.CategorieId == categorieId);
        }

        public void AddCategorieType(GedCategorieType entity)
        {
            dbContext.GedCategorieTypes.Add(entity);
        }

        public async Task<bool> MappingExistsAsync(int categorieTypeId, string fileTypeCode)
        {
            var code = fileTypeCode.Trim();
            return await dbContext.GedCategorieTypeTypesDossiers
                .AnyAsync(m => m.CategorieTypeId == categorieTypeId && m.TypeDossierCode == code);
        }

        public void AddMapping(GedCategorieTypeTypesDossier mapping)
        {
            dbContext.GedCategorieTypeTypesDossiers.Add(mapping);
        }

        public async Task<List<GedCategorieTypeTypesDossier>> GetMappingsForTypeAsync(string fileTypeCode, int typeId)
        {
            var code = fileTypeCode.Trim();
            var categorieTypeIds = dbContext.GedCategorieTypes
                .Where(ct => ct.TypesId == typeId)
                .Select(ct => ct.Id);

            return await dbContext.GedCategorieTypeTypesDossiers
                .Where(m => m.TypeDossierCode == code && categorieTypeIds.Contains(m.CategorieTypeId))
                .ToListAsync();
        }

        public void RemoveMappings(IEnumerable<GedCategorieTypeTypesDossier> mappings)
        {
            dbContext.GedCategorieTypeTypesDossiers.RemoveRange(mappings);
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

        private Guid ResolveTrackedTenant()
        {
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                switch (entry.Entity)
                {
                    case GedCategorieTypeTypesDossier map when map.TenantId != Guid.Empty:
                        return map.TenantId;
                    case GedCategorieType ct when ct.TenantId != Guid.Empty:
                        return ct.TenantId;
                    case GedDocumentType type when type.TenantId != Guid.Empty:
                        return type.TenantId;
                }
            }

            return Guid.Empty;
        }

        // Expression SQL-traductible réutilisable (compatible ToDataSourceResultAsync)
        private static readonly System.Linq.Expressions.Expression<Func<GedDocumentType, GedDocumentTypeDto>> ProjectDto =
            t => new GedDocumentTypeDto
            {
                Id = t.Id,
                TypeName = t.TypeName,
                TypeCategory = t.TypeCategory,
                UrlTemplate = t.UrlTemplate,
                TypeNameFr = t.TypeNameFr,
                TypeNameEn = t.TypeNameEn,
                TypeNameEs = t.TypeNameEs,
                TypeNamePt = t.TypeNamePt,
                CodeTemplate = t.CodeTemplate,
                RequiredValidation = t.RequiredValidation,
                DefaultFolder = t.DefaultFolder,
                TypeRole = t.TypeRole,
                Visible = t.Visible,
                UseGrCodeRecogizing = t.UseGrCodeRecogizing,
                DownloadOnlyLastVersion = t.DownloadOnlyLastVersion,
                NeedsEncryption = t.NeedsEncryption,
                TenantId = t.TenantId,
                IndustryId = t.IndustryId
            };
    }
}
