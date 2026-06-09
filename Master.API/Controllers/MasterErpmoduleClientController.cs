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
    public class MasterErpmoduleClientController : GenericController
    {
        private readonly IMasterErpmoduleClientService<MasterErpmoduleClientDTO> _service;

        public MasterErpmoduleClientController(IMasterErpmoduleClientService<MasterErpmoduleClientDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request, [FromQuery] int? tenantId)
            => Ok(await _service.GetAllByTenant(request, tenantId ?? 0));

        [HttpPost("GetAllByTenant")]
        public async Task<IActionResult> GetAllByTenant([DataSourceRequest] DataSourceRequest request, [FromQuery] int? tenantId)
            => Ok(await _service.GetAllByTenant(request, tenantId ?? 0));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MasterErpmoduleClientDTO dto)
            => Ok(await _service.CreateModuleClient(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] MasterErpmoduleClientDTO dto)
            => Ok(await _service.EditModuleClient(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveModuleClient(id));

        [HttpPost("DropDataBaseEntityBackToBack")]
        public IActionResult DropDataBaseEntityBackToBack([FromBody] object dto)
            => Ok(false);

        [HttpPost("GetAllModuleByTenantBackToBack")]
        public IActionResult GetAllModuleByTenantBackToBack([FromBody] object dto)
            => Ok(new object[0]);

        [HttpPost("GetAllModuleDatabaseByTenantBackToBack")]
        public IActionResult GetAllModuleDatabaseByTenantBackToBack([DataSourceRequest] DataSourceRequest request, [FromBody] object dto)
            => Ok(new { Data = new object[0], Total = 0 });
    }
}
