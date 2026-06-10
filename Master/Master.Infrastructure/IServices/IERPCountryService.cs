using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IErpCountryService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : ErpCountryDTO
    {
        Task<OperationResult> CreateCountry(TEntityDTO entity);
        Task<OperationResult> EditCountry(TEntityDTO entity, int id);
        Task<OperationResult> RemoveCountry(int id);
        Task<DataSourceResult> GetAllCountries(DataSourceRequest requestModel);
    }
}
