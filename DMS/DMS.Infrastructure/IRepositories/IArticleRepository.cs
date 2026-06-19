using DMS.Entities.Models;
using Kendo.Mvc.UI;
using Master.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface IArticleRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : TnArticle
    {
        // ✅ Kendo
        Task<DataSourceResult> GetAllAsync(DataSourceRequest requestModel);

        // ✅ Tes méthodes existantes
        //Task<IEnumerable<TnArticle>> GetAllAsync();
        Task<IEnumerable<TnArticle>> GetByIndustryAsync(int industryId);
        Task AddAsync(TnArticle entity);
        Task UpdateAsync(TnArticle entity);
        Task DeleteAsync(int codeArticle);
        Task<ProductServiceCategry> GetDefaultCategories();
        Task<IEnumerable<ProductServiceCategry>> GetAllCategoriesAsync();
    }
}