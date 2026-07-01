#nullable disable
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class FileTypeRepository : GenericBaseRepository<TnTypesDossier, DmsReferenceContext>, IFileTypeRepository<TnTypesDossier>
    //Hérite de GenericBaseRepository<TnTypesDossier, DmsReferenceContext> → reçoit les méthodes CRUD génériques gratuitement
    //Implémente IFileTypeRepository<TnTypesDossier> → est obligée de définir les méthodes spécifiques aux FileTypes
    {
        public FileTypeRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        // File Types par industrie
        public async Task<List<TnTypesDossier>> GetByIndustryAsync(int industryId)
        {
            return await dbContext.TnTypesDossiers
                .Include(x => x.TnActivite)
                .Where(x => x.TnActivite != null &&
                            x.TnActivite.IndustryId == industryId)
                .ToListAsync();
        }

        // File Types partagés (activité avec IndustryId == null)
        public async Task<List<TnTypesDossier>> GetSharedAsync()
        {
            return await dbContext.TnTypesDossiers
                .Include(x => x.TnActivite)
                .Where(x => x.TnActivite != null &&
                            x.TnActivite.IndustryId == null)
                .ToListAsync();
        }

        // File Type par code
        public async Task<TnTypesDossier> GetByCodeAsync(string codeTypeDossier)
        {
            return await dbContext.TnTypesDossiers
                .Include(x => x.TnActivite)
                .FirstOrDefaultAsync(x => x.CodeTypeDossier == codeTypeDossier);
        }

        // Add simple
        public async Task AddAsync(TnTypesDossier fileType)
        {
            await dbContext.TnTypesDossiers.AddAsync(fileType);
            await dbContext.SaveChangesAsync();
        }

        // Add avec création du Plan Operation en transaction
        public async Task CreateWithOperationPlanAsync(TnTypesDossier fileType, TnPlansOperation operationPlan)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Récupérer le tenant_id depuis un enregistrement existant
                var existingRecord = await dbContext.TnTypesDossiers.FirstOrDefaultAsync();
                var tenantId = existingRecord?.TenantId ?? Guid.Empty;

                // SET le tenant_id dans la session PostgreSQL
                await dbContext.Database.ExecuteSqlRawAsync(
                    $"SET LOCAL app.tenant_id = '{tenantId}'");

                // Appliquer le tenant_id aux deux entités
                fileType.TenantId = tenantId;
                operationPlan.TenantId = tenantId;

                await dbContext.TnTypesDossiers.AddAsync(fileType);
                await dbContext.TnPlansOperations.AddAsync(operationPlan);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Update
        public async Task UpdateAsync(TnTypesDossier fileType)
        {
            dbContext.TnTypesDossiers.Update(fileType);
            await dbContext.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAsync(TnTypesDossier fileType)
        {
            dbContext.TnTypesDossiers.Remove(fileType);
            await dbContext.SaveChangesAsync();
        }
    }
}