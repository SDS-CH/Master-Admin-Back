#nullable disable
using Kendo.Mvc.UI;
using Master.DTO.DTOs;
using Master.Infrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Master.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DbInstanceController : GenericController
    {
        private readonly IDbInstanceService<DbInstanceDTO> _service;

        public DbInstanceController(IDbInstanceService<DbInstanceDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<DataSourceResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => await _service.GetAllDbInstances(request);

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DbInstanceDTO dto)
            => Ok(await _service.CreateDbInstance(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] DbInstanceDTO dto)
            => Ok(await _service.EditDbInstance(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveDbInstance(id));
    }
}
