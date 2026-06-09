#nullable disable
using Kendo.Mvc.UI;
using Master.DTO;
using Master.Infrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Master.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MasterERPIndustriesClientController : GenericController
    {
        private readonly IMasterERPIndustriesClientService<MasterERPIndustriesClientDTO> _service;

        public MasterERPIndustriesClientController(IMasterERPIndustriesClientService<MasterERPIndustriesClientDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request, [FromQuery] int? clientId)
            => Ok(await _service.GetAllByTenant(request, clientId ?? 0));

        [HttpPost("GetMasterERPIndustriesClientById/{id}")]
        public async Task<IActionResult> GetMasterERPIndustriesClientById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("AttachIndustriesToClient")]
        public IActionResult AttachIndustriesToClient([FromQuery] int? clientId, [FromBody] object body)
            => Ok(new { ErrorOccured = false, Message = "Not implemented." });

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MasterERPIndustriesClientDTO dto)
            => Ok(await _service.CreateIndustryClient(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] MasterERPIndustriesClientDTO dto)
            => Ok(await _service.EditIndustryClient(dto, dto.Id));

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] MasterERPIndustriesClientDTO dto)
            => Ok(await _service.RemoveIndustryClient(dto?.Id ?? 0));
    }
}
