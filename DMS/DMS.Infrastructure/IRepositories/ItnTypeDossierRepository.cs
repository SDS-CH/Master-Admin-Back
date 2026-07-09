using DMS.Entities.Models;
using Master.Common.Interfaces;

namespace DMS.Infrastructure.IRepositories;

public interface IFileTypeRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : TnTypesDossier
{
    Task<List<TnTypesDossier>> GetByIndustryAsync(int industryId);
    Task<List<TnTypesDossier>> GetSharedAsync();
    Task<TnTypesDossier?> GetByCodeAsync(string codeTypeDossier);
    Task AddAsync(TnTypesDossier fileType);
    Task CreateWithOperationPlanAsync(TnTypesDossier fileType, TnPlansOperation operationPlan);
    Task UpdateAsync(TnTypesDossier fileType);
    Task DeleteAsync(TnTypesDossier fileType);
}