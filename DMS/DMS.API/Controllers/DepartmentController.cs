#nullable disable
using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService<DepartmentDTO> _service;

        public DepartmentController(IDepartmentService<DepartmentDTO> service)
        {
            _service = service;
        }

        [HttpPost("getAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllDepartments(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DepartmentDTO dto)
            => Ok(await _service.CreateDepartment(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] DepartmentDTO dto)
            => Ok(await _service.EditDepartment(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveDepartment(id));
    }
}
