using DMS.DTO.DTOs;
using DMS.Entities.Models;
using Kendo.Mvc.UI;
using Master.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{

    public interface ITnCodesTaxisRepository<TEntity> : IGenericBaseRepository<TEntity> where TEntity : TnCodesTaxis
    {

        Task<TEntity> GetById(string code);
        Task<DataSourceResult> GetAllTnCodesTaxis(DataSourceRequest requestModel, string countryCode = null);
        Task Delete(string code);
        Task<List<CompteComptableOptionDTO>> GetComptesByCountry(int countryId);
    }

}
