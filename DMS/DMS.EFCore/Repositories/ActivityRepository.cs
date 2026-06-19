using DMS.DTO.DTOs;
using DMS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using DMS.Infrastructure.IRepositories;

namespace DMS.EFCore.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DmsReferenceContext _context;  // ← postgresContext pas DmsDbContex

        public ActivityRepository(DmsReferenceContext context)  // ← context pas _context
        {
            _context = context;  // ← _context = context
        }

        public async Task<IEnumerable<ActivityDto>> GetAll()
        {
            return await _context.TnActivites
                .OrderBy(a => a.CodeActivite)
                .Select(a => new ActivityDto
                {
                    CodeActivite = a.CodeActivite,
                    LibelleActivite = a.LibelleActivite,
                    ModuleOperation = a.ModuleOperation,
                    ConcernFacturation = a.ConcernFacturation,
                    Session = a.Session,
                    AddNewTime = a.AddNewTime,
                    EditTime = a.EditTime,
                    TenantId = a.TenantId,
                    IndustryId = a.IndustryId
                }).ToListAsync();
        }

        public async Task Add(ActivityDto dto)
        {
            var activity = new TnActivite
            {
                CodeActivite = dto.CodeActivite,
                LibelleActivite = dto.LibelleActivite,
                ModuleOperation = dto.ModuleOperation,
                ConcernFacturation = dto.ConcernFacturation,
                Session = dto.Session,
                AddNewTime = DateTime.Now,
                EditTime = DateTime.Now,
                TenantId = dto.TenantId,
                IndustryId = dto.IndustryId
            };
            _context.TnActivites.Add(activity);
            await _context.SaveChangesAsync();
        }
    }
}