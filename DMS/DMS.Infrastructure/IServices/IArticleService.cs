using DMS.DTO.DTOs;
using DMS.Entities.Models;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IServices
{
    public interface IArticleService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : ArticleDto
    {
        // ✅ Kendo
        Task<DataSourceResult> GetAllAsync(DataSourceRequest requestModel);

        
        //Task<IEnumerable<ArticleDto>> GetAllAsync();
        Task<IEnumerable<ArticleDto>> GetByIndustryAsync(int industryId);
        Task AddAsync(ArticleDto articleDto);
        Task UpdateAsync(ArticleDto articleDto);
        Task DeleteAsync(int codeArticle);
        Task<IEnumerable<ProductServiceCategoryDto>> GetAllCategoriesAsync();
    }
}