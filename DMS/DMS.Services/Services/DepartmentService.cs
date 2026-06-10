#nullable disable
using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using System;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class DepartmentService<TDepartmentDTO, TDepartment, TContext>
        : BaseService<TDepartmentDTO, TDepartment, TContext>, IDepartmentService<TDepartmentDTO>
        where TDepartment : Department, new()
        where TDepartmentDTO : DepartmentDTO
        where TContext : DmsReferenceContext
    {
        private readonly IDepartmentRepository<TDepartment> _repository;

        public DepartmentService(TContext dbContext, IMapper mapper, IDepartmentRepository<TDepartment> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public async Task<TDepartmentDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task<DataSourceResult> GetAllDepartments(DataSourceRequest requestModel)
        {
            return await _repository.GetAllDepartments(requestModel);
        }

        public async Task<OperationResult> CreateDepartment(TDepartmentDTO entity)
        {
            try
            {
                await Create(entity, _repository);
                return new OperationResult(false, "Department created successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> EditDepartment(TDepartmentDTO entity, int id)
        {
            try
            {
                await Edit(entity, _repository);
                return new OperationResult(false, "Department updated successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }

        public async Task<OperationResult> RemoveDepartment(int id)
        {
            try
            {
                await Delete(id, _repository);
                return new OperationResult(false, "Department removed successfully.");
            }
            catch (Exception ex)
            {
                return new OperationResult(true, ex.Message);
            }
        }
    }
}
