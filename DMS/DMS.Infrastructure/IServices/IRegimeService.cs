//using DMS.DTO.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DMS.Infrastructure.IServices
//{
//    public interface IRegimeService
//    {
//        Task<List<RegimeDto>> GetRegimesByFileTypeAsync(string fileTypeCode);
//        Task<List<RegimeDto>> GetAvailableRegimesAsync(string fileTypeCode);
//        Task<RegimeDto> CreateRegimeAndLinkAsync(CreateRegimeDto dto);
//        Task<RegimeDto?> UpdateRegimeAsync(string regimeCode, UpdateRegimeDto dto);
//        Task LinkRegimeAsync(LinkRegimeDto dto);
//        Task<bool> UnlinkRegimeAsync(string fileTypeCode, string regimeCode);
//    }
//}
