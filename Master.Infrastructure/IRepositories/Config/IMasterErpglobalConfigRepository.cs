using Master.Common.Interfaces;
using Master.Entities.Models;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories.Config
{
    public interface IMasterErpglobalConfigRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : MasterErpGlobalConfigs
    {
        Task<MasterErpGlobalConfigs> GetFirstConfig();
    }
}
