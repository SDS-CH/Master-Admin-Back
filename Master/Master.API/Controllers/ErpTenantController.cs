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
    public class ErpTenantController : GenericController
    {
        private readonly IErpTenantService<ErpTenantDTO> _service;

        public ErpTenantController(IErpTenantService<ErpTenantDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllTenants(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ErpTenantDTO dto)
            => Ok(await _service.CreateTenant(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] ErpTenantDTO dto)
            => Ok(await _service.EditTenant(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveTenant(id));

        [HttpGet("GetCryptedConnections")]
        public async Task<IActionResult> GetCryptedConnections()
            => Ok(await _service.GetCryptedConnections());

        [HttpGet("GetCryptedConnectionsByTenantId/{tenantId}")]
        public async Task<IActionResult> GetCryptedConnectionsByTenantId(int tenantId)
            => Ok(await _service.GetCryptedConnectionsByTenantId(tenantId));
    }
}
