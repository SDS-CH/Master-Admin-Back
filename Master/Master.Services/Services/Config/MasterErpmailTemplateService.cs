#nullable disable
using AutoMapper;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using Master.DTO.Config;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories.Config;
using Master.Infrastructure.IServices.Config;
using System;
using System.Threading.Tasks;

namespace Master.Services.Services.Config
{
    public class MasterErpmailTemplateService<TMasterErpmailTemplateDTO, TMasterErpMailTemplates, TContext> : BaseService<TMasterErpmailTemplateDTO, TMasterErpMailTemplates, TContext>, IMasterErpmailTemplateService<TMasterErpmailTemplateDTO>
        where TMasterErpMailTemplates : MasterErpMailTemplates, new()
        where TMasterErpmailTemplateDTO : MasterErpmailTemplateDTO
        where TContext : ERPMasterContext
    {
        private readonly IMasterErpmailTemplateRepository<TMasterErpMailTemplates> _repository;

        public MasterErpmailTemplateService(TContext dbContext, IMapper mapper, IMasterErpmailTemplateRepository<TMasterErpMailTemplates> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TMasterErpmailTemplateDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<OperationResult> CreateMailTemplate(TMasterErpmailTemplateDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "MailTemplate created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditMailTemplate(TMasterErpmailTemplateDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "MailTemplate updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveMailTemplate(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "MailTemplate removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<DataSourceResult> GetAllMailTemplates(DataSourceRequest requestModel)
        {
            return await _repository.GetAll(requestModel);
        }
    }
}
