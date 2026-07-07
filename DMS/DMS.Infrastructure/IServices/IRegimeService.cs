using DMS.DTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Common.Interfaces.Services;


namespace DMS.Infrastructure.IServices
{
    public interface IRegimeService<TEntityDTO> : IBaseService<TEntityDTO> where TEntityDTO : RegimeDto
    {
        Task<List<RegimeDto>> GetAllAsync();
        Task<List<RegimeDto>> GetByFileTypeAsync(string codeTypeDossier);
        Task CreateAsync(CreateRegimeDto dto);
        Task LinkAsync(LinkRegimeDto dto);
        Task UnlinkAsync(string codeTypeDossier, string codeRegime);
    }
}
