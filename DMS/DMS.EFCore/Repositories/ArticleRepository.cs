#nullable disable
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.EFCore.Repositories
{
    public class ArticleRepository : GenericBaseRepository<TnArticle, DmsReferenceContext>, IArticleRepository<TnArticle>
    {
        public ArticleRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        // ✅ Kendo
        public async Task<DataSourceResult> GetAllAsync(DataSourceRequest requestModel)
        {
            return await dbContext.TnArticles.ToDataSourceResultAsync(requestModel);
        }

        

        // ✅ GET BY INDUSTRY
        public async Task<IEnumerable<TnArticle>> GetByIndustryAsync(int industryId)
        {
            return await dbContext.TnArticles
                .Where(x => x.IndustryId == industryId)
                .ToListAsync();
        }

        // ✅ ADD
        public async Task AddAsync(TnArticle entity)
        {
            var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"SET LOCAL app.tenant_id = '{tenantId}'");
            entity.LibelleAuto = false;
            entity.Session = 0;
            entity.TenantId = tenantId;
            entity.AddNewTime = DateTime.UtcNow;
            entity.EditTime = DateTime.UtcNow;
            await dbContext.TnArticles.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        // ✅ UPDATE
        public async Task UpdateAsync(TnArticle entity)
        {
            var existing = await dbContext.TnArticles
                .FirstOrDefaultAsync(x => x.CodeArticle == entity.CodeArticle);
            if (existing == null) return;
            var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"SET LOCAL app.tenant_id = '{tenantId}'");
            existing.LibelleArticle = entity.LibelleArticle;
            existing.DescriptionArticle = entity.DescriptionArticle;
            existing.CategorieArticle = entity.CategorieArticle;
            existing.IndustryId = entity.IndustryId;
            existing.TenantId = tenantId;
            existing.EditTime = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        // ✅ DELETE
        public async Task DeleteAsync(int codeArticle)
        {
            var entity = await dbContext.TnArticles
                .FirstOrDefaultAsync(x => x.CodeArticle == codeArticle);
            if (entity == null) return;
            var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            await dbContext.Database.ExecuteSqlRawAsync($"SET LOCAL app.tenant_id = '{tenantId}'");
            dbContext.TnArticles.Remove(entity);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        // ✅ GET CATEGORIES BY INDUSTRY
        public async Task<ProductServiceCategry> GetDefaultCategories()
        {
            return await dbContext.ProductServiceCategries
                .Where(x =>   x.IsDefault == true)
                .FirstOrDefaultAsync();

           
        }

        // ✅ GET ALL CATEGORIES
        public async Task<IEnumerable<ProductServiceCategry>> GetAllCategoriesAsync()
        {
            return await dbContext.ProductServiceCategries
                .Where(x => x.IsDefault == false)
                .ToListAsync();
        }
    }
}