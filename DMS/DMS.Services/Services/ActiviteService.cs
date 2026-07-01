using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;

namespace DMS.Application.Services
{
    public class ActiviteService : IActiviteService
    {
        private readonly IActiviteRepository _repository;

        public ActiviteService(IActiviteRepository repository)
        {
            _repository = repository;
        }

        // Activités par industrie
        public async Task<List<ActiviteDto>> GetByIndustryAsync(int industryId)
        {
            var activites = await _repository.GetByIndustryAsync(industryId);
            return activites.Select(x => new ActiviteDto
            {
                CodeActivite = x.CodeActivite,
                LibelleActivite = x.LibelleActivite,
                IndustryId = x.IndustryId
            }).ToList();
        }

        // Activités partagées (IndustryId == null)
        public async Task<List<ActiviteDto>> GetSharedAsync()
        {
            var activites = await _repository.GetSharedAsync();
            return activites.Select(x => new ActiviteDto
            {
                CodeActivite = x.CodeActivite,
                LibelleActivite = x.LibelleActivite,
                IndustryId = x.IndustryId
            }).ToList();
        }

        // Activité par code
        public async Task<ActiviteDto?> GetByCodeAsync(string codeActivite)
        {
            var activite = await _repository.GetByCodeAsync(codeActivite);
            if (activite == null)
                return null;
            return new ActiviteDto
            {
                CodeActivite = activite.CodeActivite,
                LibelleActivite = activite.LibelleActivite,
                IndustryId = activite.IndustryId
            };
        }
    }
}