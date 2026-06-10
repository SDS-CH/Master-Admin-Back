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
    public class MasterERPIndustriesController : GenericController
    {
        private readonly IMasterERPIndustriesService<MasterERPIndustriesDTO> _service;

        public MasterERPIndustriesController(IMasterERPIndustriesService<MasterERPIndustriesDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request, [FromQuery] int? clientId)
            => Ok(await _service.GetAllIndustries(request));

        [HttpPost("ReadAll")]
        public async Task<IActionResult> ReadAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllIndustries(request));

        [HttpPost("GetMasterERPIndustriesById/{id}")]
        public async Task<IActionResult> GetMasterERPIndustriesById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MasterERPIndustriesDTO dto)
            => Ok(await _service.CreateIndustry(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] MasterERPIndustriesDTO dto)
            => Ok(await _service.EditIndustry(dto, dto.Id));

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
            => Ok(await _service.RemoveIndustry(id));
    }
}
