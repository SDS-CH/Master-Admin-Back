#nullable disable
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using System.Threading.Tasks;

namespace DMS.EFCore.Repositories
{
    public class DepartmentRepository : GenericBaseRepository<Department, DmsReferenceContext>, IDepartmentRepository<Department>
    {
        public DepartmentRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<DataSourceResult> GetAllDepartments(DataSourceRequest requestModel)
        {
            return await dbContext.Departments.ToDataSourceResultAsync(requestModel);
        }
    }
}
