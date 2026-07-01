using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Entities.Models;

namespace DMS.Infrastructure.IRepositories
{
    public interface IActiviteRepository
    {
        Task<List<TnActivite>> GetByIndustryAsync(int industryId);
        Task<List<TnActivite>> GetSharedAsync(); // IndustryId == null
        Task<TnActivite?> GetByCodeAsync(string codeActivite);
    }
}