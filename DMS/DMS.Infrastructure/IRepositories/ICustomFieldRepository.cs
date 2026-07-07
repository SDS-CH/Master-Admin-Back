using DMS.Entities.Models;
using Master.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface ICustomFieldRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : TnCodesComplementsDossier
    {
        Task<List<TnCodesComplementsDossier>> GetAllAsync();
        Task<List<(TnCodesComplementsDossier CustomField, bool IsRequiredOnFileClosure)>> GetByFileTypeAsync(string codeTypeDossier);
        Task<TnCodesComplementsDossier> GetByCodeAsync(string codeComplement);
        Task AddAsync(TnCodesComplementsDossier customField);
        Task<bool> IsLinkedAsync(string codeTypeDossier, string complementCode);
        Task LinkToFileTypeAsync(string codeTypeDossier, string complementCode, bool isRequiredOnFileClosure);
        Task UnlinkFromFileTypeAsync(string codeTypeDossier, string complementCode);
        Task<bool> UpdateIsRequiredAsync(string codeTypeDossier, string complementCode, bool isRequiredOnFileClosure);
    }
}
