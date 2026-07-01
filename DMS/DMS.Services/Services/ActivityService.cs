using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Master.Common.Classes.Services;

namespace DMS.Services.Services
{
    public class ActivityService<TActivityDTO, TActivity, TContext>
        : BaseService<TActivityDTO, TActivity, TContext>, IActivityService<TActivityDTO>
        where TActivity : TnActivite, new()
        where TActivityDTO : ActivityDto
        where TContext : DmsReferenceContext
    {
        private readonly IActivityRepository<TActivity> _repository;

        public ActivityService(TContext dbContext, IMapper mapper, IActivityRepository<TActivity> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TActivityDTO>> GetAll(int? industryId = null, bool unassignedOnly = false)
        {
            var activities = await _repository.GetAllActivities(industryId, unassignedOnly);
            return _mapper.Map<IEnumerable<TActivityDTO>>(activities);
        }

        public async Task Add(TActivityDTO dto)
        {
            var now = DateTime.Now;
            dto.CodeActivite = await GenerateUniqueActivityCode(dto.TenantId, dto.CodeActivite);
            dto.ModuleOperation = string.IsNullOrWhiteSpace(dto.ModuleOperation) ? "XXX" : dto.ModuleOperation.Trim();
            dto.AddNewTime = dto.AddNewTime == default ? now : dto.AddNewTime;
            dto.EditTime = now;

            await Create(dto, _repository);
        }

        public async Task<bool> Update(string codeActivite, Guid tenantId, TActivityDTO dto)
        {
            dto.CodeActivite = codeActivite;
            dto.TenantId = tenantId;
            dto.EditTime = DateTime.Now;

            var activity = _mapper.Map<TActivity>(dto);
            var updated = await _repository.UpdateActivity(codeActivite, tenantId, activity);
            return updated is not null;
        }

        public async Task<bool> Delete(string codeActivite, Guid tenantId)
        {
            return await _repository.DeleteActivity(codeActivite, tenantId);
        }

        private async Task<string> GenerateUniqueActivityCode(Guid tenantId, string? requestedCode)
        {
            var activities = await _repository.GetAllActivities();
            var existingCodes = activities
                .Where(activity => activity.TenantId == tenantId)
                .Select(activity => activity.CodeActivite?.Trim().ToUpperInvariant())
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .ToHashSet();

            var baseCode = NormalizeActivityCode(requestedCode);
            var candidate = baseCode;
            var suffix = 2;

            while (existingCodes.Contains(candidate))
            {
                var suffixText = suffix.ToString();
                var maxBaseLength = Math.Max(1, 10 - suffixText.Length);
                candidate = $"{baseCode[..Math.Min(baseCode.Length, maxBaseLength)]}{suffixText}";
                suffix++;
            }

            return candidate;
        }

        private static string NormalizeActivityCode(string? code)
        {
            var normalized = string.IsNullOrWhiteSpace(code) ? "ACT" : code.Trim().ToUpperInvariant();
            normalized = new string(normalized
                .Where(ch => char.IsLetterOrDigit(ch) || ch == '_')
                .ToArray());

            if (string.IsNullOrWhiteSpace(normalized))
            {
                normalized = "ACT";
            }

            return normalized[..Math.Min(normalized.Length, 10)];
        }
    }
}
