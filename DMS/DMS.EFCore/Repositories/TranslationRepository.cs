using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DMS.EFCore.Repositories
{
    public class TranslationRepository : ITranslationRepository
    {
        private readonly DmsReferenceContext _context;

        public TranslationRepository(DmsReferenceContext context)
        {
            _context = context;
        }

        public async Task<DataSourceResult> GetByMenuAsync(string menu, DataSourceRequest request)
        {
            var data = await _context.DbFunctions.fn_translations_by_menuAsync(menu);
            return await data.AsQueryable().ToDataSourceResultAsync(request);
        }

        public async Task UpdateAsync(int id, string translatedLabel)
        {
            var entity = await _context.TranslationData
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return;
            entity.TranslatedLabel = translatedLabel;
            await _context.SaveChangesAsync();
        }
    }
}