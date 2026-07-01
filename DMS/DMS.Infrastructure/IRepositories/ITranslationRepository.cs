using DMS.Entities.Models;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface ITranslationRepository
    {
        Task<DataSourceResult> GetByMenuAsync(string menu, DataSourceRequest request);
        Task UpdateAsync(int id, string translatedLabel);
    }
}