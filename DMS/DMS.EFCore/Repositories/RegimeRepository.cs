using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

#nullable disable
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class RegimeRepository : GenericBaseRepository<TnCodesRegime, DmsReferenceContext>, IRegimeRepository<TnCodesRegime>
    {
        public RegimeRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        // Référentiel global — pour le dropdown "Select Regimes..."
        public async Task<List<TnCodesRegime>> GetAllAsync()
        {
            return await dbContext.TnCodesRegimes
                .OrderBy(x => x.Label)
                .ToListAsync();
        }

        // Regimes liés à un FileType — via la navigation property TnCodesRegime
        public async Task<List<TnCodesRegime>> GetByFileTypeAsync(string codeTypeDossier)
        {
            return await dbContext.TnFileTypeRegimes
                .Include(x => x.TnCodesRegime)
                .Where(x => x.FileType == codeTypeDossier)
                .Select(x => x.TnCodesRegime)
                .OrderBy(x => x.Label)
                .ToListAsync();
        }

        public async Task<TnCodesRegime> GetByCodeAsync(string codeRegime)
        {
            return await dbContext.TnCodesRegimes
                .FirstOrDefaultAsync(x => x.CodeRegime == codeRegime);
        }

        // Même pattern tenant_id que CreateWithOperationPlanAsync (FileType)
        public async Task AddAsync(TnCodesRegime regime)
        {
            var existingRecord = await dbContext.TnCodesRegimes.FirstOrDefaultAsync();
            var tenantId = existingRecord?.TenantId ?? Guid.Empty;

            await dbContext.Database.ExecuteSqlRawAsync(
                $"SET LOCAL app.tenant_id = '{tenantId}'");

            regime.TenantId = tenantId;

            await dbContext.TnCodesRegimes.AddAsync(regime);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsLinkedAsync(string codeTypeDossier, string codeRegime)
        {
            return await dbContext.TnFileTypeRegimes
                .AnyAsync(x => x.FileType == codeTypeDossier && x.RegimeCode == codeRegime);
        }

        // Lien idempotent (validé ensemble) : si déjà lié → on ne fait rien
        public async Task LinkToFileTypeAsync(string codeTypeDossier, string codeRegime)
        {
            var alreadyLinked = await IsLinkedAsync(codeTypeDossier, codeRegime);
            if (alreadyLinked) return;

            var existingRecord = await dbContext.TnFileTypeRegimes.FirstOrDefaultAsync();
            var tenantId = existingRecord?.TenantId ?? Guid.Empty;

            await dbContext.Database.ExecuteSqlRawAsync(
                $"SET LOCAL app.tenant_id = '{tenantId}'");

            var link = new TnFileTypeRegime
            {
                FileType = codeTypeDossier,
                RegimeCode = codeRegime,
                TenantId = tenantId
            };

            await dbContext.TnFileTypeRegimes.AddAsync(link);
            await dbContext.SaveChangesAsync();
        }

        // Détache uniquement le lien (FileType + RegimeCode) — ne touche pas tn_Codes_Regimes
        public async Task UnlinkFromFileTypeAsync(string codeTypeDossier, string codeRegime)
        {
            var link = await dbContext.TnFileTypeRegimes
                .FirstOrDefaultAsync(x => x.FileType == codeTypeDossier && x.RegimeCode == codeRegime);

            if (link == null) return;

            dbContext.TnFileTypeRegimes.Remove(link);
            await dbContext.SaveChangesAsync();
        }
    }
}