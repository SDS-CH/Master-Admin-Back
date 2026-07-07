using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class ActivityRepository : GenericBaseRepository<TnActivite, DmsReferenceContext>, IActivityRepository<TnActivite>
    {
        public ActivityRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<List<TnActivite>> GetAllActivities(int? industryId = null, bool unassignedOnly = false)
        {
            var query = dbContext.TnActivites.AsNoTracking();

            if (unassignedOnly)
            {
                query = query.Where(a => a.IndustryId == null);
            }
            else if (industryId.HasValue)
            {
                query = query.Where(a => a.IndustryId == industryId.Value);
            }

            return await query.OrderBy(a => a.CodeActivite).ToListAsync();
        }

        public async Task<TnActivite?> UpdateActivity(string codeActivite, Guid tenantId, TnActivite activity)
        {
            var existing = await dbContext.TnActivites.FindAsync(codeActivite, tenantId);
            if (existing is null)
            {
                return null;
            }

            existing.LibelleActivite = activity.LibelleActivite;

            // Ne pas écraser ModuleOperation si le frontend n'envoie rien
            // (évite la violation de contrainte FK vers tn_Modules_Operations)
            if (!string.IsNullOrWhiteSpace(activity.ModuleOperation))
            {
                existing.ModuleOperation = activity.ModuleOperation;
            }

            // Même logique de préservation pour ConcernFacturation
            if (!string.IsNullOrWhiteSpace(activity.ConcernFacturation))
            {
                existing.ConcernFacturation = activity.ConcernFacturation;
            }

            existing.Session = activity.Session;
            existing.EditTime = activity.EditTime;
            existing.IndustryId = activity.IndustryId;

            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteActivity(string codeActivite, Guid tenantId)
        {
            var deleted = await Delete(new object[] { codeActivite, tenantId });
            return deleted > 0;
        }
    }
}