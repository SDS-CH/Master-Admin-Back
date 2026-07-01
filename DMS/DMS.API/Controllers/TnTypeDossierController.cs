using DMS.Infrastructure.IServices;
using DMS.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileTypesController : ControllerBase
    {
        private readonly IFileTypeService<FileTypeDto> _service;

        public FileTypesController(IFileTypeService<FileTypeDto> service)
        {
            _service = service;
        }

        [HttpGet("industry/{industryId}")]
        public async Task<IActionResult> GetByIndustry(int industryId)
        {
            var result = await _service.GetByIndustryAsync(industryId);
            return Ok(result);
        }

        [HttpGet("shared")]
        public async Task<IActionResult> GetShared()
        {
            var result = await _service.GetSharedAsync();
            return Ok(result);
        }

        [HttpGet("{codeTypeDossier}")]
        public async Task<IActionResult> GetByCode(string codeTypeDossier)
        {
            var result = await _service.GetByCodeAsync(codeTypeDossier);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // ↓ MODIFIÉ — retourne l'inner exception
        [HttpPost]
        public async Task<IActionResult> Add(FileTypeDto dto)
        {
            try
            {
                await _service.AddAsync(dto);
                return Ok(new { message = "File Type added successfully" });
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

        [HttpPut("{codeTypeDossier}")]
        public async Task<IActionResult> Update(string codeTypeDossier, FileTypeDto dto)
        {
            try
            {
                await _service.UpdateAsync(codeTypeDossier, dto);
                return Ok(new { message = "File Type updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{codeTypeDossier}")]
        public async Task<IActionResult> Delete(string codeTypeDossier)
        {
            try
            {
                await _service.DeleteAsync(codeTypeDossier);
                return Ok(new { message = "File Type deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}