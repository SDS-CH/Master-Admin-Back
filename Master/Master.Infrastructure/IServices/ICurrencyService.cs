using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface ICurrencyService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : CurrencyDTO
    {
        Task<OperationResult> CreateCurrency(TEntityDTO entity);
        Task<OperationResult> EditCurrency(TEntityDTO entity, int id);
        Task<OperationResult> RemoveCurrency(int id);
        Task<DataSourceResult> GetAllCurrencies(DataSourceRequest requestModel);
    }
}
