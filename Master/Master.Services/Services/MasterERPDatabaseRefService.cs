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
    public class MasterERPDatabaseRefService<TMasterERPDatabaseRefDTO, TMasterErpDatabaseRef, TContext> : BaseService<TMasterERPDatabaseRefDTO, TMasterErpDatabaseRef, TContext>, IMasterERPDatabaseRefService<TMasterERPDatabaseRefDTO>
        where TMasterErpDatabaseRef : MasterErpDatabaseRef, new()
        where TMasterERPDatabaseRefDTO : MasterERPDatabaseRefDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterERPDatabaseRefRepository<TMasterErpDatabaseRef> _repository;

        public MasterERPDatabaseRefService(TContext dbContext, IMapper mapper, IMasterERPDatabaseRefRepository<TMasterErpDatabaseRef> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterERPDatabaseRefDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateDatabaseRef(TMasterERPDatabaseRefDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "DatabaseRef created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditDatabaseRef(TMasterERPDatabaseRefDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "DatabaseRef updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveDatabaseRef(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "DatabaseRef removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllDatabaseRefs(DataSourceRequest requestModel)
        {
            return await _repository.GetAllDatabaseRefs(requestModel);
        }
    }
}
