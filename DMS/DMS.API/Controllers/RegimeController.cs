using DMS.Infrastructure.IServices;
using DMS.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegimesController : ControllerBase
    {
        private readonly IRegimeService<RegimeDto> _service;

        public RegimesController(IRegimeService<RegimeDto> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("filetype/{codeTypeDossier}")]
        public async Task<IActionResult> GetByFileType(string codeTypeDossier)
        {
            var result = await _service.GetByFileTypeAsync(codeTypeDossier);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRegimeDto dto)
        {
            try
            {
                await _service.CreateAsync(dto);
                return Ok(new { message = "Regime created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    inner2 = ex.InnerException?.InnerException?.Message
                });
            }
        }

        [HttpPost("link")]
        public async Task<IActionResult> Link(LinkRegimeDto dto)
        {
            try
            {
                await _service.LinkAsync(dto);
                return Ok(new { message = "Regime(s) linked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("unlink/{codeTypeDossier}/{codeRegime}")]
        public async Task<IActionResult> Unlink(string codeTypeDossier, string codeRegime)
        {
            try
            {
                await _service.UnlinkAsync(codeTypeDossier, codeRegime);
                return Ok(new { message = "Regime unlinked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}