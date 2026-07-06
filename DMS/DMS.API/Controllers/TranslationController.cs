#nullable disable
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationService _service;

        public TranslationController(ITranslationService service)
        {
            _service = service;
        }

        [HttpPost("{menu}")]
        public async Task<IActionResult> GetByMenu(string menu, [DataSourceRequest] DataSourceRequest request)
        {
            var result = await _service.GetByMenuAsync(menu, request);
            return Ok(result);
        }

        [HttpPut("fr/{id}")]
        public async Task<IActionResult> UpdateFr(int id, [FromBody] string translatedLabel)
        {
            await _service.UpdateAsync(id, translatedLabel);
            return Ok();
        }

        [HttpPut("de/{id}")]
        public async Task<IActionResult> UpdateDe(int id, [FromBody] string translatedLabel)
        {
            await _service.UpdateAsync(id, translatedLabel);
            return Ok();
        }
    }
}