using DMS.Infrastructure.IServices;
using DMS.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomFieldsController : ControllerBase
    {
        private readonly ICustomFieldService<CustomFieldDto> _service;

        public CustomFieldsController(ICustomFieldService<CustomFieldDto> service)
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

        [HttpGet("types")]
        public IActionResult GetAvailableTypes()
        {
            var types = _service.GetAvailableTypes();
            return Ok(types);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomFieldDto dto)
        {
            try
            {
                await _service.CreateAsync(dto);
                return Ok(new { message = "Custom Field created successfully" });
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
        public async Task<IActionResult> Link(LinkCustomFieldDto dto)
        {
            try
            {
                await _service.LinkAsync(dto);
                return Ok(new { message = "Custom Field(s) linked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{codeTypeDossier}/{complementCode}")]
        public async Task<IActionResult> UpdateIsRequired(string codeTypeDossier, string complementCode, [FromBody] bool isRequiredOnFileClosure)
        {
            var updated = await _service.UpdateIsRequiredAsync(codeTypeDossier, complementCode, isRequiredOnFileClosure);
            return updated ? Ok(new { message = "Updated successfully" }) : NotFound();
        }

        [HttpDelete("unlink/{codeTypeDossier}/{complementCode}")]
        public async Task<IActionResult> Unlink(string codeTypeDossier, string complementCode)
        {
            try
            {
                await _service.UnlinkAsync(codeTypeDossier, complementCode);
                return Ok(new { message = "Custom Field unlinked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}