#nullable disable
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class CustomFieldRepository : GenericBaseRepository<TnCodesComplementsDossier, DmsReferenceContext>, ICustomFieldRepository<TnCodesComplementsDossier>
    {
        public CustomFieldRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        // Référentiel global — pour le dropdown "Select"
        public async Task<List<TnCodesComplementsDossier>> GetAllAsync()
        {
            return await dbContext.TnCodesComplementsDossiers
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        // Custom Fields liés à un FileType — JOIN manuel (pas de navigation property)
        public async Task<List<(TnCodesComplementsDossier CustomField, bool IsRequiredOnFileClosure)>> GetByFileTypeAsync(string codeTypeDossier)
        {
            var result = await (
                from link in dbContext.TnFileComplementsRequiredOptions
                join cf in dbContext.TnCodesComplementsDossiers
                    on link.ComplementDossier equals cf.CodeComplement
                where link.TypeDossier == codeTypeDossier
                orderby cf.Name
                select new { cf, link.IsRequiredOnFileClosure }
            ).ToListAsync();

            return result.Select(x => (x.cf, x.IsRequiredOnFileClosure)).ToList();
        }

        public async Task<TnCodesComplementsDossier> GetByCodeAsync(string codeComplement)
        {
            return await dbContext.TnCodesComplementsDossiers
                .FirstOrDefaultAsync(x => x.CodeComplement == codeComplement);
        }

        // Même pattern tenant_id que Regime/FileType
        public async Task AddAsync(TnCodesComplementsDossier customField)
        {
            var existingRecord = await dbContext.TnCodesComplementsDossiers.FirstOrDefaultAsync();
            var tenantId = existingRecord?.TenantId ?? Guid.Empty;

            await dbContext.Database.ExecuteSqlRawAsync(
                $"SET LOCAL app.tenant_id = '{tenantId}'");

            customField.TenantId = tenantId;

            await dbContext.TnCodesComplementsDossiers.AddAsync(customField);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsLinkedAsync(string codeTypeDossier, string complementCode)
        {
            return await dbContext.TnFileComplementsRequiredOptions
                .AnyAsync(x => x.TypeDossier == codeTypeDossier && x.ComplementDossier == complementCode);
        }

        // Lien idempotent : si déjà lié → on ne fait rien
        public async Task LinkToFileTypeAsync(string codeTypeDossier, string complementCode, bool isRequiredOnFileClosure)
        {
            var alreadyLinked = await IsLinkedAsync(codeTypeDossier, complementCode);
            if (alreadyLinked) return;

            var existingRecord = await dbContext.TnFileComplementsRequiredOptions.FirstOrDefaultAsync();
            var tenantId = existingRecord?.TenantId ?? Guid.Empty;

            await dbContext.Database.ExecuteSqlRawAsync(
                $"SET LOCAL app.tenant_id = '{tenantId}'");

            var link = new TnFileComplementsRequiredOption
            {
                TypeDossier = codeTypeDossier,
                ComplementDossier = complementCode,
                IsRequiredOnFileCreation = false,
                IsRequiredOnFileClosure = isRequiredOnFileClosure,
                TenantId = tenantId
            };

            await dbContext.TnFileComplementsRequiredOptions.AddAsync(link);
            await dbContext.SaveChangesAsync();
        }

        // Détache uniquement le lien — ne touche pas au référentiel global
        public async Task UnlinkFromFileTypeAsync(string codeTypeDossier, string complementCode)
        {
            var link = await dbContext.TnFileComplementsRequiredOptions
                .FirstOrDefaultAsync(x => x.TypeDossier == codeTypeDossier && x.ComplementDossier == complementCode);

            if (link == null) return;

            dbContext.TnFileComplementsRequiredOptions.Remove(link);
            await dbContext.SaveChangesAsync();
        }

        // Update — seulement le flag IsRequiredOnFileClosure
        public async Task<bool> UpdateIsRequiredAsync(string codeTypeDossier, string complementCode, bool isRequiredOnFileClosure)
        {
            var link = await dbContext.TnFileComplementsRequiredOptions
                .FirstOrDefaultAsync(x => x.TypeDossier == codeTypeDossier && x.ComplementDossier == complementCode);

            if (link == null) return false;

            link.IsRequiredOnFileClosure = isRequiredOnFileClosure;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}