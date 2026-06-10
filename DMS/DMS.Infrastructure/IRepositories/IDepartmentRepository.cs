using DMS.Entities.Models;
using Kendo.Mvc.UI;
using Master.Common.Interfaces;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface IDepartmentRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : Department
    {
        Task<DataSourceResult> GetAllDepartments(DataSourceRequest requestModel);
    }
}
