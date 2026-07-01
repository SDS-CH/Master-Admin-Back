using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/Activities")]
    public class ActivitesController : ControllerBase
    {
        private readonly IActiviteService _service;

        public ActivitesController(IActiviteService service)
        {
            _service = service;
        }

        // GET api/Activities/industry/1
        [HttpGet("industry/{industryId}")]
        public async Task<IActionResult> GetByIndustry(int industryId)
        {
            var result = await _service.GetByIndustryAsync(industryId);
            return Ok(result);
        }

        // GET api/Activities/shared
        [HttpGet("shared")]
        public async Task<IActionResult> GetShared()
        {
            var result = await _service.GetSharedAsync();
            return Ok(result);
        }

        // GET api/Activities/IMP
        [HttpGet("{codeActivite}")]
        public async Task<IActionResult> GetByCode(string codeActivite)
        {
            var result = await _service.GetByCodeAsync(codeActivite);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}