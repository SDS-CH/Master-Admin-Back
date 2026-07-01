using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.DTO.DTOs;

namespace DMS.Infrastructure.IServices
{
    public interface IActiviteService
    {
        Task<List<ActiviteDto>> GetByIndustryAsync(int industryId);
        Task<List<ActiviteDto>> GetSharedAsync(); // IndustryId == null
        Task<ActiviteDto?> GetByCodeAsync(string codeActivite);
    }
}