using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/ged-documents")]
    public class GedDocumentCategoryController : ControllerBase
    {
        private readonly IGedDocumentCategoryService<GedDocumentCategoryDto> _service;

        public GedDocumentCategoryController(IGedDocumentCategoryService<GedDocumentCategoryDto> service)
        {
            _service = service;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _service.GetCategories());
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes([FromQuery] int categoryId, [FromQuery] int? industryId, [FromQuery] bool unassignedOnly = false)
        {
            return Ok(await _service.GetTypes(categoryId, industryId, unassignedOnly));
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateType([FromBody] GedDocumentTypeDto dto)
        {
            var result = await _service.CreateType(dto);
            return result.Success ? Ok(result.Data) : BadRequest(new { message = result.Error });
        }

        [HttpPut("types/{id:int}")]
        public async Task<IActionResult> UpdateType(int id, [FromBody] GedDocumentTypeDto dto)
        {
            var result = await _service.UpdateType(id, dto);

            if (!result.Success && result.Error == "Document type not found.")
            {
                return NotFound();
            }

            return result.Success ? Ok(result.Data) : BadRequest(new { message = result.Error });
        }

        [HttpDelete("types/{id:int}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var deleted = await _service.DeleteType(id);
            return deleted ? Ok(new { message = "Document type deleted." }) : NotFound();
        }
    }
}
