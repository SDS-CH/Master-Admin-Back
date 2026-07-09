using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DMS.API.Controllers
{
    [ApiController]
    [Route("api/FileTypes")]
    public class FileTypeDocumentTypesController : ControllerBase
    {
        private readonly IFileTypeDocumentTypeService<GedDocumentTypeDto> _service;

        public FileTypeDocumentTypesController(IFileTypeDocumentTypeService<GedDocumentTypeDto> service)
        {
            _service = service;
        }

        // Types de documents rattachés à un type de dossier (grille)
        [HttpGet("{fileTypeCode}/document-types")]
        public async Task<IActionResult> GetAttached(string fileTypeCode)
        {
            var result = await _service.GetAttachedTypesAsync(fileTypeCode);
            return Ok(result);
        }

        // Recherche serveur (Kendo DataSourceRequest) des types disponibles à rattacher (multiselect)
        [HttpPost("{fileTypeCode}/document-types/search")]
        public async Task<IActionResult> Search(string fileTypeCode, [DataSourceRequest] DataSourceRequest request)
        {
            var result = await _service.SearchTypesAsync(fileTypeCode, request);
            return Ok(result);
        }

        // Rattacher un ou plusieurs types (depuis la master data)
        [HttpPost("{fileTypeCode}/document-types")]
        public async Task<IActionResult> Attach(string fileTypeCode, [FromBody] AttachDocumentTypesDto dto)
        {
            var result = await _service.AttachTypesAsync(fileTypeCode, dto);
            return ToActionResult(result);
        }

        // Détacher un type du type de dossier
        [HttpDelete("{fileTypeCode}/document-types/{typeId:int}")]
        public async Task<IActionResult> Detach(string fileTypeCode, int typeId)
        {
            var result = await _service.DetachTypeAsync(fileTypeCode, typeId);
            return ToActionResult(result);
        }

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
