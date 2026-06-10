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
    public class DbInstanceService<TDbInstanceDTO, TDbInstance, TContext> : BaseService<TDbInstanceDTO, TDbInstance, TContext>, IDbInstanceService<TDbInstanceDTO>
        where TDbInstance : DbInstance, new()
        where TDbInstanceDTO : DbInstanceDTO
        where TContext : ERPMasterContext
    {
        private readonly IDbInstanceRepository<TDbInstance> _repository;

        public DbInstanceService(TContext dbContext, IMapper mapper, IDbInstanceRepository<TDbInstance> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TDbInstanceDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateDbInstance(TDbInstanceDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "DbInstance created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditDbInstance(TDbInstanceDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "DbInstance updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveDbInstance(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "DbInstance removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllDbInstances(DataSourceRequest requestModel)
        {
            return await _repository.GetAll(requestModel);
        }
    }
}
