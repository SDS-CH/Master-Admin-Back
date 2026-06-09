using Master.Common.Interfaces;
using Master.Entities.Models;

namespace Master.Infrastructure.IRepositories.Config
{
    public interface IMasterErpmailTemplateRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpMailTemplates
    {
    }
}
