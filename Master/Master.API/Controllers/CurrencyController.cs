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
    public class CurrencyController : GenericController
    {
        private readonly ICurrencyService<CurrencyDTO> _service;

        public CurrencyController(ICurrencyService<CurrencyDTO> service)
        {
            _service = service;
        }

        [HttpPost("getCurrency")]
        public async Task<IActionResult> GetCurrency([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllCurrencies(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CurrencyDTO dto)
            => Ok(await _service.CreateCurrency(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] CurrencyDTO dto)
            => Ok(await _service.EditCurrency(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveCurrency(id));
    }
}
