using Master.Common.Interfaces;
using Master.Entities.Models;

namespace Master.Infrastructure.IRepositories
{
    public interface IDbInstanceRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : DbInstance
    {
    }
}
