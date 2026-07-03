using System.Collections.Generic;
using System.Threading.Tasks;
using DMS.Entities.Models;

using Master.Common.Interfaces;

namespace DMS.Infrastructure.IRepositories
{
    public interface IRegimeRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : TnCodesRegime
    {
        Task<List<TnCodesRegime>> GetAllAsync();
        Task<List<TnCodesRegime>> GetByFileTypeAsync(string codeTypeDossier);
        Task<TnCodesRegime> GetByCodeAsync(string codeRegime);
        Task AddAsync(TnCodesRegime regime);
        Task<bool> IsLinkedAsync(string codeTypeDossier, string codeRegime);
        Task LinkToFileTypeAsync(string codeTypeDossier, string codeRegime);
        Task UnlinkFromFileTypeAsync(string codeTypeDossier, string codeRegime);
    }
}