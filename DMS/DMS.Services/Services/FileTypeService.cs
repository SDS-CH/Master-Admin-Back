using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Master.Common.Classes.Services;

namespace DMS.Application.Services
{
    public class FileTypeService<TFileTypeDTO, TFileType, TContext>
        : BaseService<TFileTypeDTO, TFileType, TContext>, IFileTypeService<TFileTypeDTO>
        where TFileType : TnTypesDossier, new()
        where TFileTypeDTO : FileTypeDto
        where TContext : DmsReferenceContext
    {
        private readonly IFileTypeRepository<TFileType> _repository;
        private readonly IActiviteRepository _activiteRepository;

        public FileTypeService(
            TContext dbContext,
            IMapper mapper,
            IFileTypeRepository<TFileType> repository,
            IActiviteRepository activiteRepository)
            : base(dbContext, mapper)
        {
            _repository = repository;
            _activiteRepository = activiteRepository;
        }

        // GET BY INDUSTRY
        public async Task<List<FileTypeDto>> GetByIndustryAsync(int industryId)
        {
            var fileTypes = await _repository.GetByIndustryAsync(industryId);
            return fileTypes.Select(x => new FileTypeDto
            {
                CodeTypeDossier = x.CodeTypeDossier,
                LibelleTypeDossier = x.LibelleTypeDossier,
                Activite = x.Activite,
                ActiviteLibelle = x.TnActivite != null ? x.TnActivite.LibelleActivite : null
            }).ToList();
        }

        // GET SHARED (IndustryId == null)
        public async Task<List<FileTypeDto>> GetSharedAsync()
        {
            var fileTypes = await _repository.GetSharedAsync();
            return fileTypes.Select(x => new FileTypeDto
            {
                CodeTypeDossier = x.CodeTypeDossier,
                LibelleTypeDossier = x.LibelleTypeDossier,
                Activite = x.Activite,
                ActiviteLibelle = x.TnActivite != null ? x.TnActivite.LibelleActivite : null
            }).ToList();
        }

        // GET BY CODE
        public async Task<FileTypeDto?> GetByCodeAsync(string codeTypeDossier)
        {
            var fileType = await _repository.GetByCodeAsync(codeTypeDossier);
            if (fileType == null)
                return null;
            return new FileTypeDto
            {
                CodeTypeDossier = fileType.CodeTypeDossier,
                LibelleTypeDossier = fileType.LibelleTypeDossier,
                Activite = fileType.Activite,
                ActiviteLibelle = fileType.TnActivite != null ? fileType.TnActivite.LibelleActivite : null
            };
        }

        // ADD — génère le code + crée le Plan Operation automatiquement
        public async Task AddAsync(FileTypeDto dto)
        {
            // Vérifier si le libellé existe déjà
            var existing = await _repository.GenericGetFirstOrDefaultAsync(
                e => e.LibelleTypeDossier == dto.LibelleTypeDossier);
            if (existing != null)
                throw new Exception("Un File Type avec ce libellé existe déjà.");

            // Récupérer l'activité pour avoir ModuleOperation
            var activity = await _activiteRepository.GetByCodeAsync(dto.Activite);
            if (activity == null)
                throw new Exception("Activité introuvable.");

            // Générer le code (7 chars)
            var code = Guid.NewGuid().ToString().Substring(0, 7).ToUpper();

            // Créer le File Type
            var newFileType = new TnTypesDossier
            {
                CodeTypeDossier = code,
                LibelleTypeDossier = dto.LibelleTypeDossier,
                Activite = dto.Activite,
                SensTrafic = "",
                ModeTransport = "",
                Mensuel = false,
                PlanOperationOuverture = "D" + code,
                Disponible = true,
                CustomValueLabel = "",
                Session = 0,
                AddNewTime = DateTime.Now,
                EditTime = DateTime.Now,
                TenantId = Guid.Empty
            };

            // Créer le Plan Operation associé
            var operationPlan = new TnPlansOperation
            {
                CodePlanOperation = "D" + code,
                Flag = -1,
                ValidationImmediate = false,
                ValiditeRequise = false,
                Regroupement = "Ouverture Dossier",
                LibellePlanOperation = "Open File - " + dto.LibelleTypeDossier,
                ModeTransport = "",
                SensTrafic = "",
                Session = 0,
                ModuleOperation = activity.ModuleOperation ?? "",
                AddNewTime = DateTime.Now,
                EditTime = DateTime.Now,
                TenantId = Guid.Empty
            };

            await _repository.CreateWithOperationPlanAsync(newFileType, operationPlan);
        }

        // UPDATE — seulement LibelleTypeDossier et Activite
        public async Task UpdateAsync(string codeTypeDossier, FileTypeDto dto)
        {
            var entity = await _repository.GetByCodeAsync(codeTypeDossier);
            if (entity == null)
                throw new Exception("File Type not found");

            entity.LibelleTypeDossier = dto.LibelleTypeDossier;
            entity.Activite = dto.Activite;
            entity.EditTime = DateTime.Now;

            await _repository.UpdateAsync(entity);
        }

        // DELETE
        public async Task DeleteAsync(string codeTypeDossier)
        {
            var entity = await _repository.GetByCodeAsync(codeTypeDossier);
            if (entity == null)
                throw new Exception("File Type not found");

            await _repository.DeleteAsync(entity);
        }

        public Task<TFileTypeDTO> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}