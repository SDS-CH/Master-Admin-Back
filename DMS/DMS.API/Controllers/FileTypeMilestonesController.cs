using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/FileTypes")]
    public class FileTypeMilestonesController : ControllerBase
    {
        private readonly IFileTypeMilestonesService _service;

        public FileTypeMilestonesController(IFileTypeMilestonesService service)
        {
            _service = service;
        }

        [HttpGet("{fileTypeCode}/milestones")]
        public async Task<IActionResult> GetMappedMilestones(string fileTypeCode)
        {
            var result = await _service.GetMappedMilestonesAsync(fileTypeCode);
            if (result is null)
            {
                return NotFound(new { message = "File type not found." });
            }
            return Ok(result);
        }

        [HttpGet("{fileTypeCode}/milestones/search")]
        public async Task<IActionResult> SearchMilestones(string fileTypeCode, [FromQuery] string? search)
        {
            var result = await _service.SearchMilestonesAsync(fileTypeCode, search);
            if (result is null)
            {
                return NotFound(new { message = $"File type '{fileTypeCode}' not found." });
            }
            return Ok(result);
        }

        [HttpGet("milestones/search")]
        public async Task<IActionResult> SearchMilestonesByQuery([FromQuery] string fileTypeCode, [FromQuery] string? search)
        {
            var result = await _service.SearchMilestonesAsync(fileTypeCode, search);
            if (result is null)
            {
                return NotFound(new { message = $"File type '{fileTypeCode}' not found." });
            }
            return Ok(result);
        }

        [HttpPost("{fileTypeCode}/milestones")]
        public async Task<IActionResult> AddMilestones(string fileTypeCode, [FromBody] AddFileTypeMilestonesDto dto)
        {
            var error = await _service.AddMilestonesAsync(fileTypeCode, dto);
            if (!string.IsNullOrEmpty(error))
            {
                if (error.Contains("already mapped"))
                {
                    return Ok(new { message = error });
                }
                return BadRequest(new { message = error });
            }
            return Ok(new { message = "Milestones added successfully." });
        }

        [HttpPost("{fileTypeCode}/milestones/steps")]
        public async Task<IActionResult> CreateStep(string fileTypeCode, [FromBody] CreateMilestoneStepDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LibelleEtape))
            {
                return BadRequest(new { message = "Step label is required." });
            }

            try
            {
                var result = await _service.CreateStepAsync(fileTypeCode, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.GetBaseException().Message });
            }
        }

        [HttpPut("milestones/{mappingId}")]
        public async Task<IActionResult> UpdateMilestoneMapping(int mappingId, [FromBody] UpdateFileTypeMilestoneDto dto)
        {
            var success = await _service.UpdateMilestoneMappingAsync(mappingId, dto);
            if (!success)
            {
                return NotFound(new { message = "Milestone mapping not found." });
            }
            return Ok(new { message = "Milestone mapping updated successfully." });
        }

        [HttpDelete("milestones/{mappingId}")]
        public async Task<IActionResult> DeleteMilestoneMapping(int mappingId)
        {
            var success = await _service.DeleteMilestoneMappingAsync(mappingId);
            if (!success)
            {
                return NotFound(new { message = "Milestone mapping not found." });
            }
            return Ok(new { message = "Milestone unmapped successfully." });
        }
    }
}
