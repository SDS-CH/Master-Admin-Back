using DMS.DTO.DTOs;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IServices
{
    public interface IDepartmentService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : DepartmentDTO
    {
        Task<DataSourceResult> GetAllDepartments(DataSourceRequest requestModel);
        Task<OperationResult> CreateDepartment(TEntityDTO entity);
        Task<OperationResult> EditDepartment(TEntityDTO entity, int id);
        Task<OperationResult> RemoveDepartment(int id);
    }
}
