using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using Master.DTO.DTOs;
using Kendo.Mvc.UI;
using System.Threading.Tasks;

namespace Master.Infrastructure.IServices
{
    public interface IGedDirectoryService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : GedDirectoryDTO
    {
        Task<OperationResult> CreateGedDirectory(TEntityDTO entity);
        Task<OperationResult> EditGedDirectory(TEntityDTO entity, int id);
        Task<OperationResult> RemoveGedDirectory(int id);
        Task<DataSourceResult> GetAllGedDirectories(DataSourceRequest requestModel);
    }
}
