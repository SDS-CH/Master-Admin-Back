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
    public class MasterErpmodulesController : GenericController
    {
        private readonly IMasterErpmodulesService<MasterErpmodulesDTO> _service;

        public MasterErpmodulesController(IMasterErpmodulesService<MasterErpmodulesDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllModules(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MasterErpmodulesDTO dto)
            => Ok(await _service.CreateModule(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] MasterErpmodulesDTO dto)
            => Ok(await _service.EditModule(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveModule(id));
    }
}
