#nullable disable
using Kendo.Mvc.UI;
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
    public class ErpUserController : GenericController
    {
        private readonly IErpUserService<ErpUserDTO> _service;

        public ErpUserController(IErpUserService<ErpUserDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllUsers(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("GetByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
            => Ok(await _service.GetByEmail(email));

        [HttpPost("GetUsersByTenant")]
        public async Task<DataSourceResult> GetUsersByTenant([DataSourceRequest] DataSourceRequest request, [FromQuery] int tenantId)
        {
            return await _service.GetUsersByTenant(request, tenantId);
        }

        [HttpPost("GetErpUsersByTenantSimple")]
        public async Task<IActionResult> GetErpUsersByTenantSimple([FromBody] ErpUserDTO dto)
            => Ok(await _service.GetErpUsersByTenant(dto?.Id ?? 0));

        [HttpPost("GetIsErpUser")]
        public async Task<IActionResult> GetIsErpUser([FromBody] ErpUserDTO dto)
        {
            if (dto == null) return Ok(new { ErrorOccured = true, Message = "Invalid request" });
            var user = await _service.GetByEmail(dto.Email ?? "");
            return Ok(new { ErrorOccured = false, Data = user != null && user.IsErpUser });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ErpUserDTO dto)
            => Ok(await _service.CreateUser(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] ErpUserDTO dto)
            => Ok(await _service.EditUser(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveUser(id));

        [HttpPost("VerifEmailExist")]
        public async Task<IActionResult> VerifEmailExist([FromBody] ErpUserDTO dto)
            => Ok(await _service.VerifEmailExist(dto?.Email ?? ""));

        [HttpPost("CreateUserForTenant/{tenantId}")]
        public async Task<IActionResult> CreateUserForTenant(int tenantId, [FromBody] ErpUserDTO dto)
            => Ok(await _service.CreateUserForTenant(dto, tenantId));

        [HttpPost("CreateOrAttach/{tenantId}")]
        public async Task<IActionResult> CreateOrAttach(int tenantId, [FromBody] ErpUserDTO dto)
            => Ok(await _service.CreateOrAttachUserForTenant(dto, tenantId));

        [HttpPost("Deactivate/{tenantId}/{userId}")]
        public async Task<IActionResult> Deactivate(int tenantId, int userId)
            => Ok(await _service.DeactivateUserForTenant(userId, tenantId));

        [HttpPost("Activate/{userId}")]
        public async Task<IActionResult> Activate(int userId, [FromQuery] int tenantId) 
        {
            var result = await _service.ActivateUserForTenant(userId, tenantId);
            return Ok(result);
        }

        [HttpPost("SyncForTenant")]
        public IActionResult SyncForTenant([FromBody] object dto)
            => Ok(new { ErrorOccured = false, Message = "Sync not implemented." });
    }
}
