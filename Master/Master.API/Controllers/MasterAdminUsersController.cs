#nullable disable
using Master.DTO.Users;
using Master.Infrastructure.IServices.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Master.API.Controllers
{
    [ApiController]
    [Authorize]
    public class MasterAdminUsersController : GenericController
    {
        private readonly IUserService<MasterAdminUsersDTO> _service;

        public MasterAdminUsersController(IUserService<MasterAdminUsersDTO> service)
        {
            _service = service;
        }

        [HttpGet("/usersnt/{adminId}")]
        public async Task<IActionResult> GetById(int adminId)
            => Ok(await _service.GetById(adminId));

        [HttpGet("/users/current")]
        public async Task<IActionResult> GetCurrent()
        {
            GetCurrentUserIdFromClaim();
            return Ok(await _service.GetById(currentUserId));
        }

        [HttpGet("/users/photo")]
        public IActionResult GetCurrentPhoto()
            => NoContent();

        [HttpPost("/usersnt/Create")]
        public async Task<IActionResult> Create([FromBody] MasterAdminUsersDTO dto)
            => Ok(await _service.CreateUser(dto));

        [HttpPost("/usersnt/Edit")]
        public async Task<IActionResult> Edit([FromBody] MasterAdminUsersDTO dto)
            => Ok(await _service.EditUser(dto, dto.Id));

        [HttpPost("/usersnt/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveUser(id));
    }
}
