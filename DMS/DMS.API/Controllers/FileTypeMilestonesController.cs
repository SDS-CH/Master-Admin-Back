using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Master.Common.Classes;
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
        private readonly IFileTypeMilestonesService<MilestoneStepDto> _service;

        public FileTypeMilestonesController(IFileTypeMilestonesService<MilestoneStepDto> service)
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
            var result = await _service.AddMilestonesAsync(fileTypeCode, dto);
            return ToActionResult(result);
        }

        [HttpPost("{fileTypeCode}/milestones/steps")]
        public async Task<IActionResult> CreateStep(string fileTypeCode, [FromBody] CreateMilestoneStepDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LibelleEtape))
            {
                return BadRequest(new { message = "Step label is required." });
            }

            var result = await _service.CreateStepAsync(fileTypeCode, dto);
            return ToActionResult(result);
        }

        [HttpPut("milestones/{mappingId}")]
        public async Task<IActionResult> UpdateMilestoneMapping(int mappingId, [FromBody] UpdateFileTypeMilestoneDto dto)
        {
            var result = await _service.UpdateMilestoneMappingAsync(mappingId, dto);
            return ToActionResult(result);
        }

        [HttpDelete("milestones/{mappingId}")]
        public async Task<IActionResult> DeleteMilestoneMapping(int mappingId)
        {
            var result = await _service.DeleteMilestoneMappingAsync(mappingId);
            return ToActionResult(result);
        }

        // Mappe un OperationResult vers la réponse HTTP appropriée
        private IActionResult ToActionResult(OperationResult result)
        {
            if (!result.ErrorOccured)
            {
                return Ok(result);
            }

            if (!string.IsNullOrEmpty(result.Message) &&
                result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(result);
            }

            return BadRequest(result);
        }
    }
}
