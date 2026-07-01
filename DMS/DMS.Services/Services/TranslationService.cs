using DMS.DTO.DTOs;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly ITranslationRepository _repository;

        public TranslationService(ITranslationRepository repository)
        {
            _repository = repository;
        }

        public async Task<DataSourceResult> GetByMenuAsync(string menu, DataSourceRequest request)
        {
            return await _repository.GetByMenuAsync(menu, request);
        }

        public async Task UpdateAsync(int id, string translatedLabel)
        {
            await _repository.UpdateAsync(id, translatedLabel);
        }
    }
}