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
    public class ERPCountryController : GenericController
    {
        private readonly IErpCountryService<ErpCountryDTO> _service;

        public ERPCountryController(IErpCountryService<ErpCountryDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAllCountries")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllCountries(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ErpCountryDTO dto)
            => Ok(await _service.CreateCountry(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] ErpCountryDTO dto)
            => Ok(await _service.EditCountry(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveCountry(id));
    }
}
