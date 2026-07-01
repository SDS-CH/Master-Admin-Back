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
            existing.ModuleOperation = activity.ModuleOperation;
            existing.ConcernFacturation = activity.ConcernFacturation;
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
