using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TnCodesTaxisController : ControllerBase
    {
        private readonly ITnCodesTaxisService<TnCodesTaxisDTO> _service;

        public TnCodesTaxisController(ITnCodesTaxisService<TnCodesTaxisDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request, [FromQuery] string countryCode = null)
            => Ok(await _service.GetAllTnCodesTaxis(request, countryCode));

        [HttpPost("GetById/{code}")]
        public async Task<IActionResult> GetById(string code)
            => Ok(await _service.GetById(code));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TnCodesTaxisDTO dto)
            => Ok(await _service.CreateTnCodesTaxis(dto));

        [HttpPost("Edit/{code}")]
        public async Task<IActionResult> Edit(string code, [FromBody] TnCodesTaxisDTO dto)
    => Ok(await _service.EditTnCodesTaxis(dto, code));

        [HttpPost("Delete/{code}")]
        public async Task<IActionResult> Delete(string code)
            => Ok(await _service.RemoveTnCodesTaxis(code));
    }
}
