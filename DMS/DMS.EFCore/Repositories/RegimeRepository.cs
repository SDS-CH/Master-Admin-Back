//using DMS.Entities.Models;
//using DMS.Infrastructure.IRepositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

//namespace DMS.EFCore.Repositories
//{
//    public class RegimeRepository : IRegimeRepository
//    {
//        private readonly DmsReferenceContext _context;

//        public RegimeRepository(DmsReferenceContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<TnCodesRegime>> GetRegimesByFileTypeAsync(string fileTypeCode)
//        {
//            return await _context.TnFileTypeRegimes
//                .Where(ftr => ftr.FileType == fileTypeCode)
//                .Include(ftr => ftr.TnCodesRegime)
//                .Select(ftr => ftr.TnCodesRegime)
//                .Where(r => r != null)
//                .ToListAsync();
//        }

//        public async Task<List<TnCodesRegime>> GetAvailableRegimesAsync(string fileTypeCode)
//        {
//            var linkedCodes = _context.TnFileTypeRegimes
//                .Where(ftr => ftr.FileType == fileTypeCode)
//                .Select(ftr => ftr.RegimeCode);

//            return await _context.TnCodesRegimes
//                .Where(r => !linkedCodes.Contains(r.CodeRegime))
//                .OrderBy(r => r.Label)
//                .ToListAsync();
//        }

//        public async Task<bool> RegimeCodeExistsAsync(string regimeCode)
//        {
//            return await _context.TnCodesRegimes
//                .AnyAsync(r => r.CodeRegime == regimeCode);
//        }

//        public async Task<TnCodesRegime> CreateRegimeAsync(TnCodesRegime regime)
//        {
//            // tenant_id est injecté automatiquement par la RLS Postgres à l'insertion,
//            // comme pour les autres entités multi-tenant de l'app (TnActivite, TnTypesDossier...).
//            _context.TnCodesRegimes.Add(regime);
//            await _context.SaveChangesAsync();
//            return regime;
//        }

//        public async Task<TnCodesRegime?> UpdateRegimeAsync(string regimeCode, string? label, string? descriptionRegime, string? acronym)
//        {
//            var regime = await _context.TnCodesRegimes
//                .FirstOrDefaultAsync(r => r.CodeRegime == regimeCode);

//            if (regime == null) return null;

//            regime.Label = label;
//            regime.DescriptionRegime = descriptionRegime;
//            regime.Acronym = acronym;

//            await _context.SaveChangesAsync();
//            return regime;
//        }

//        public async Task LinkRegimeToFileTypeAsync(string fileTypeCode, string regimeCode)
//        {
//            var link = new TnFileTypeRegime
//            {
//                FileType = fileTypeCode,
//                RegimeCode = regimeCode
//            };
//            _context.TnFileTypeRegimes.Add(link);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<bool> UnlinkRegimeFromFileTypeAsync(string fileTypeCode, string regimeCode)
//        {
//            var link = await _context.TnFileTypeRegimes
//                .FirstOrDefaultAsync(ftr => ftr.FileType == fileTypeCode && ftr.RegimeCode == regimeCode);

//            if (link == null) return false;

//            _context.TnFileTypeRegimes.Remove(link);
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> IsRegimeLinkedToFileTypeAsync(string fileTypeCode, string regimeCode)
//        {
//            return await _context.TnFileTypeRegimes
//                .AnyAsync(ftr => ftr.FileType == fileTypeCode && ftr.RegimeCode == regimeCode);
//        }
//    }
//}