using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IMasterERPIndustriesClientService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : MasterERPIndustriesClientDTO
    {
        Task<OperationResult> CreateIndustryClient(TEntityDTO entity);
        Task<OperationResult> EditIndustryClient(TEntityDTO entity, int id);
        Task<OperationResult> RemoveIndustryClient(int id);
        Task<DataSourceResult> GetAllByTenant(DataSourceRequest requestModel, int tenantId);
    }
}
