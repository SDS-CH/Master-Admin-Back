using DMS.Entities.Models;
using Master.Common.Interfaces;

namespace DMS.Infrastructure.IRepositories
{
    public interface IActivityRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : TnActivite
    {
        Task<List<TEntity>> GetAllActivities(int? industryId = null, bool unassignedOnly = false);
        Task<TEntity?> UpdateActivity(string codeActivite, Guid tenantId, TEntity activity);
        Task<bool> DeleteActivity(string codeActivite, Guid tenantId);
    }
}
