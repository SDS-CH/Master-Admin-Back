#nullable disable
using AutoMapper;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO.DTOs;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Master.Infrastructure.IServices;
using System;
using System.Threading.Tasks;

namespace Master.Services.Services
{
    public class GedDirectoryService<TGedDirectoryDTO, TGedDirectory, TContext> : BaseService<TGedDirectoryDTO, TGedDirectory, TContext>, IGedDirectoryService<TGedDirectoryDTO>
        where TGedDirectory : GedDirectory, new()
        where TGedDirectoryDTO : GedDirectoryDTO
        where TContext : ERPMasterContext
    {
        private readonly IGedDirectoryRepository<TGedDirectory> _repository;

        public GedDirectoryService(TContext dbContext, IMapper mapper, IGedDirectoryRepository<TGedDirectory> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TGedDirectoryDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateGedDirectory(TGedDirectoryDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "GedDirectory created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditGedDirectory(TGedDirectoryDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "GedDirectory updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveGedDirectory(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "GedDirectory removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllGedDirectories(DataSourceRequest requestModel)
        {
            return await _repository.GetAll(requestModel);
        }
    }
}
