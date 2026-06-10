using Master.Common.Interfaces;
using Master.Entities.Models;
using System.Threading.Tasks;

namespace Master.Infrastructure.IRepositories
{
    public interface IRegisterAuthCodeRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : RegisterAuthCodes
    {
        Task<RegisterAuthCodes> GetByEmail(string email);
    }
}
