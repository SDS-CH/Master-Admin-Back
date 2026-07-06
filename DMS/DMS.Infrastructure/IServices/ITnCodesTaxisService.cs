using DMS.DTO.DTOs;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IServices
{
    public interface ITnCodesTaxisService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : TnCodesTaxisDTO
    {
        Task<DataSourceResult> GetAllTnCodesTaxis(DataSourceRequest requestModel, string countryCode = null);
        Task<TEntityDTO> GetById(string code);
        Task<OperationResult> CreateTnCodesTaxis(TEntityDTO entity);
        Task<OperationResult> EditTnCodesTaxis(TEntityDTO entity, string code);
        Task<OperationResult> RemoveTnCodesTaxis(string code);
        Task<List<CompteComptableOptionDTO>> GetComptesByCountry(int countryId);
    }
}
