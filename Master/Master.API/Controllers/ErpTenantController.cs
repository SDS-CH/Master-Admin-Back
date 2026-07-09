#nullable disable
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.DTO.DTOs;
using Master.Infrastructure.IServices;
using Master.Services.Services;
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
        public async Task<DataSourceResult> GetAll([FromBody] DataSourceRequest request)
         => await _service.GetAllTenants(request);

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<OperationResult> CreateTenant([FromBody] ErpTenantDTO tenantDto)
        {
            var result = await _service.CreateTenant(tenantDto);

            return result;
        }
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
