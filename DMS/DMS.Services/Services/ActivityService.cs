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
            var libelle = dto.LibelleActivite?.Trim();
            if (string.IsNullOrWhiteSpace(libelle))
            {
                throw new ArgumentException("Le libellé de l'activité est obligatoire.");
            }
            if (libelle.Length > 100)
            {
                throw new ArgumentException("Le libellé de l'activité ne doit pas dépasser 100 caractères.");
            }

            dto.LibelleActivite = libelle;

            var now = DateTime.Now;
            var codeBase = GenerateActivityCodeBase(libelle);
            dto.CodeActivite = await GenerateUniqueActivityCode(dto.TenantId, codeBase);
            dto.ModuleOperation = string.IsNullOrWhiteSpace(dto.ModuleOperation) ? "XXX" : dto.ModuleOperation.Trim();
            dto.AddNewTime = dto.AddNewTime == default ? now : dto.AddNewTime;
            dto.EditTime = now;

            await Create(dto, _repository);
        }

        public async Task<bool> Update(string codeActivite, Guid tenantId, TActivityDTO dto)
        {
            var libelle = dto.LibelleActivite?.Trim();
            if (string.IsNullOrWhiteSpace(libelle))
            {
                throw new ArgumentException("Le libellé de l'activité est obligatoire.");
            }
            if (libelle.Length > 100)
            {
                throw new ArgumentException("Le libellé de l'activité ne doit pas dépasser 100 caractères.");
            }

            dto.LibelleActivite = libelle;
            dto.CodeActivite = codeActivite;
            dto.TenantId = tenantId;
            dto.EditTime = DateTime.Now;
            // dto.ModuleOperation reste tel quel (vide si non fourni) ;
            // la préservation de l'ancienne valeur est gérée dans ActivityRepository.UpdateActivity

            var activity = _mapper.Map<TActivity>(dto);
            var updated = await _repository.UpdateActivity(codeActivite, tenantId, activity);
            return updated is not null;
        }

        public async Task<bool> Delete(string codeActivite, Guid tenantId)
        {
            return await _repository.DeleteActivity(codeActivite, tenantId);
        }

        // ── Génère la base du code à partir des initiales du libellé ──
        // "Advisory & Counseling" -> "AC"
        // "Consulting"            -> "CO"
        // ""                      -> "ACT" (fallback)
        private static string GenerateActivityCodeBase(string libelle)
        {
            var trimmed = libelle?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmed))
            {
                return "ACT";
            }

            var words = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                return "ACT";
            }

            string codeBase = words.Length >= 2
                ? $"{words[0][0]}{words[1][0]}".ToUpperInvariant()
                : (words[0].Length >= 2
                    ? words[0].Substring(0, 2).ToUpperInvariant()
                    : words[0].ToUpperInvariant());

            codeBase = new string(codeBase.Where(char.IsLetterOrDigit).ToArray());
            return string.IsNullOrEmpty(codeBase) ? "ACT" : codeBase;
        }

        // ── Assure l'unicité du code par tenant, en ajoutant un suffixe si besoin ──
        // "AC" existe déjà -> "AC2", "AC3", ...
        private async Task<string> GenerateUniqueActivityCode(Guid tenantId, string codeBase)
        {
            var activities = await _repository.GetAllActivities();
            var existingCodes = activities
                .Where(activity => activity.TenantId == tenantId)
                .Select(activity => activity.CodeActivite?.Trim().ToUpperInvariant())
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .ToHashSet();

            var normalizedBase = NormalizeActivityCode(codeBase);
            var candidate = normalizedBase;
            var suffix = 2;

            while (existingCodes.Contains(candidate))
            {
                var suffixText = suffix.ToString();
                var maxBaseLength = Math.Max(1, 10 - suffixText.Length);
                candidate = $"{normalizedBase[..Math.Min(normalizedBase.Length, maxBaseLength)]}{suffixText}";
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