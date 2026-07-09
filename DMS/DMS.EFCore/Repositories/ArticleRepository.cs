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
    return await dbContext.TnArticles
        .Where(x => x.IndustryId == null)
        .OrderBy(x => x.CodeArticle)
        .ToDataSourceResultAsync(requestModel);
}

        

        //  GET BY INDUSTRY
        public async Task<IEnumerable<TnArticle>> GetByIndustryAsync(int industryId)
        {
            return await dbContext.TnArticles
                .Where(x => x.IndustryId == industryId)
                 .OrderBy(x => x.CodeArticle)
                .ToListAsync();
        }

        //  ADD
        public async Task AddAsync(TnArticle entity)
        {
            var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            await dbContext.Database.ExecuteSqlRawAsync(
                "SELECT set_config('app.tenant_id', {0}, true)", tenantId.ToString());
            entity.LibelleAuto = false;
            entity.Session = 0;
            entity.TenantId = tenantId;
            entity.AddNewTime = DateTime.UtcNow;
            entity.EditTime = DateTime.UtcNow;
            await dbContext.TnArticles.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        //  UPDATE
        public async Task UpdateAsync(TnArticle entity)
        {
            var existing = await dbContext.TnArticles
                .AsNoTracking()  
                .FirstOrDefaultAsync(x => x.CodeArticle == entity.CodeArticle);

            if (existing == null) return;

            var tenantId = existing.TenantId; 

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await dbContext.Database.ExecuteSqlRawAsync(
                    "SELECT set_config('app.tenant_id', {0}, true)", tenantId.ToString());

                await dbContext.Database.ExecuteSqlRawAsync(
                    @"UPDATE dms_reference.""tn_Articles"" SET 
                ""Libelle Article"" = {0},
                ""Description Article"" = {1},
                ""Categorie Article"" = {2},
                ""industryId"" = {3}
              WHERE ""Code Article"" = {4}",
                    entity.LibelleArticle,
                    entity.DescriptionArticle,
                    entity.CategorieArticle,
                    entity.IndustryId,
                    entity.CodeArticle
                );

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //  DELETE
        public async Task DeleteAsync(int codeArticle)
        {
            var entity = await dbContext.TnArticles
                .AsNoTracking()  
                .FirstOrDefaultAsync(x => x.CodeArticle == codeArticle);

            if (entity == null) return;

            var tenantId = entity.TenantId; 

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await dbContext.Database.ExecuteSqlRawAsync(
                    "SELECT set_config('app.tenant_id', {0}, true)", tenantId.ToString());

                await dbContext.Database.ExecuteSqlRawAsync(
                    @"DELETE FROM dms_reference.""tn_Articles"" WHERE ""Code Article"" = {0}",
                    codeArticle
                );

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //  GET CATEGORIES BY INDUSTRY
        public async Task<ProductServiceCategry> GetDefaultCategories()
        {
            return await dbContext.ProductServiceCategries
                .Where(x =>   x.IsDefault == true)
                .FirstOrDefaultAsync();

           
        }

        //  GET ALL CATEGORIES
        public async Task<IEnumerable<ProductServiceCategry>> GetAllCategoriesAsync()
        {
            return await dbContext.ProductServiceCategries
                .Where(x => x.IsDefault == false)
                .ToListAsync();
        }
    }
}