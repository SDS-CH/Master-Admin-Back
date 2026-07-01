using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IServices
{
    public interface ITranslationService
    {
        Task<DataSourceResult> GetByMenuAsync(string menu, DataSourceRequest request);
        Task UpdateAsync(int id, string translatedLabel);
    }
}