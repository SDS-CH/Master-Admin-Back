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
    public class MasterERPDatabaseRefController : GenericController
    {
        private readonly IMasterERPDatabaseRefService<MasterERPDatabaseRefDTO> _service;

        public MasterERPDatabaseRefController(IMasterERPDatabaseRefService<MasterERPDatabaseRefDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllDatabaseRefs(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetMasterERPDatabaseRefById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MasterERPDatabaseRefDTO dto)
            => Ok(await _service.CreateDatabaseRef(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] MasterERPDatabaseRefDTO dto)
            => Ok(await _service.EditDatabaseRef(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveDatabaseRef(id));
    }
}
