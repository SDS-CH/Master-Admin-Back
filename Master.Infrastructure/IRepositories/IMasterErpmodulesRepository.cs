using Master.Common.Interfaces;
using Master.Entities.Models;

namespace Master.Infrastructure.IRepositories
{
    public interface IMasterErpmodulesRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpModules
    {
    }
}
