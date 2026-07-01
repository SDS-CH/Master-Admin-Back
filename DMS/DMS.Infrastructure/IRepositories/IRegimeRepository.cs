//using System.Collections.Generic;
//using System.Threading.Tasks;
//using DMS.Entities.Models;

//namespace DMS.Infrastructure.IRepositories
//{
//    public interface IRegimeRepository
//    {
//        /// <summary>Régimes déjà liés à ce file type (pour la grille).</summary>
//        Task<List<TnCodesRegime>> GetRegimesByFileTypeAsync(string fileTypeCode);

//        /// <summary>Régimes existants MAIS PAS ENCORE liés à ce file type (pour le dropdown "Select Regimes...").</summary>
//        Task<List<TnCodesRegime>> GetAvailableRegimesAsync(string fileTypeCode);

//        Task<bool> RegimeCodeExistsAsync(string regimeCode);

//        Task<TnCodesRegime> CreateRegimeAsync(TnCodesRegime regime);

//        Task<TnCodesRegime?> UpdateRegimeAsync(string regimeCode, string? label, string? descriptionRegime, string? acronym);

//        Task LinkRegimeToFileTypeAsync(string fileTypeCode, string regimeCode);

//        Task<bool> UnlinkRegimeFromFileTypeAsync(string fileTypeCode, string regimeCode);

//        Task<bool> IsRegimeLinkedToFileTypeAsync(string fileTypeCode, string regimeCode);
//    }
//}