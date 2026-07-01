using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class ActiviteRepository : IActiviteRepository
    {
        private readonly DmsReferenceContext _context;

        public ActiviteRepository(DmsReferenceContext context)
        {
            _context = context;
        }

        // Activités par industrie
        public async Task<List<TnActivite>> GetByIndustryAsync(int industryId)
        {
            return await _context.TnActivites
                .Where(x => x.IndustryId == industryId)
                .OrderBy(x => x.CodeActivite)
                .ToListAsync();
        }

        // Activités partagées (IndustryId == null)
        public async Task<List<TnActivite>> GetSharedAsync()
        {
            return await _context.TnActivites
                .Where(x => x.IndustryId == null)
                .OrderBy(x => x.CodeActivite)
                .ToListAsync();
        }

        // Activité par code
        public async Task<TnActivite?> GetByCodeAsync(string codeActivite)
        {
            return await _context.TnActivites
                .FirstOrDefaultAsync(x => x.CodeActivite == codeActivite);
        }
    }
}