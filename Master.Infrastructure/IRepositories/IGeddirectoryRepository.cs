using Master.Common.Interfaces;
using Master.Entities.Models;

namespace Master.Infrastructure.IRepositories
{
    public interface IGedDirectoryRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : GedDirectory
    {
    }
}
