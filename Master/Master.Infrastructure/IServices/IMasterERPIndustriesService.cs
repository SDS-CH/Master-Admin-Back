using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IMasterERPIndustriesService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterERPIndustriesDTO
    {
        Task<OperationResult> CreateIndustry(TEntityDTO entity);
        Task<OperationResult> EditIndustry(TEntityDTO entity, int id);
        Task<OperationResult> RemoveIndustry(int id);
        Task<DataSourceResult> GetAllIndustries(DataSourceRequest requestModel);
    }
}
