//using DMS.DTO.DTOs;
//using DMS.Infrastructure.IServices;
//using Microsoft.AspNetCore.Mvc;

//namespace DMS.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class RegimesController : ControllerBase
//    {
//        private readonly IRegimeService _regimeService;

//        public RegimesController(IRegimeService regimeService)
//        {
//            _regimeService = regimeService;
//        }

//        // GET api/Regimes/filetype/FCADV01
//        // Régimes déjà liés à ce file type (pour la grille)
//        [HttpGet("filetype/{fileTypeCode}")]
//        public async Task<IActionResult> GetByFileType(string fileTypeCode)
//        {
//            if (string.IsNullOrWhiteSpace(fileTypeCode))
//                return BadRequest("Le code du File Type est requis.");

//            var regimes = await _regimeService.GetRegimesByFileTypeAsync(fileTypeCode);
//            return Ok(regimes);
//        }

//        // GET api/Regimes/available/FCADV01
//        // Régimes existants mais pas encore liés (pour le dropdown "Select Regimes...")
//        [HttpGet("available/{fileTypeCode}")]
//        public async Task<IActionResult> GetAvailable(string fileTypeCode)
//        {
//            if (string.IsNullOrWhiteSpace(fileTypeCode))
//                return BadRequest("Le code du File Type est requis.");

//            var regimes = await _regimeService.GetAvailableRegimesAsync(fileTypeCode);
//            return Ok(regimes);
//        }

//        // POST api/Regimes
//        // Crée un nouveau régime ET le lie automatiquement au FileTypeCode fourni
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateRegimeDto dto)
//        {
//            try
//            {
//                var created = await _regimeService.CreateRegimeAndLinkAsync(dto);
//                return CreatedAtAction(nameof(GetByFileType), new { fileTypeCode = dto.FileTypeCode }, created);
//            }
//            catch (InvalidOperationException ex)
//            {
//                return Conflict(new { message = ex.Message });
//            }
//        }

//        // PUT api/Regimes/DIAG
//        [HttpPut("{regimeCode}")]
//        public async Task<IActionResult> Update(string regimeCode, [FromBody] UpdateRegimeDto dto)
//        {
//            var updated = await _regimeService.UpdateRegimeAsync(regimeCode, dto);
//            if (updated == null)
//                return NotFound(new { message = $"Régime \"{regimeCode}\" introuvable." });

//            return Ok(updated);
//        }

//        // POST api/Regimes/link
//        // Lie un régime EXISTANT à un file type (depuis le dropdown "Select Regimes...")
//        [HttpPost("link")]
//        public async Task<IActionResult> Link([FromBody] LinkRegimeDto dto)
//        {
//            try
//            {
//                await _regimeService.LinkRegimeAsync(dto);
//                return Ok();
//            }
//            catch (InvalidOperationException ex)
//            {
//                return Conflict(new { message = ex.Message });
//            }
//        }

//        // DELETE api/Regimes/unlink/FCADV01/DIAG
//        // Retire le lien entre un régime et un file type (ne supprime pas le régime lui-même)
//        [HttpDelete("unlink/{fileTypeCode}/{regimeCode}")]
//        public async Task<IActionResult> Unlink(string fileTypeCode, string regimeCode)
//        {
//            var removed = await _regimeService.UnlinkRegimeAsync(fileTypeCode, regimeCode);
//            if (!removed)
//                return NotFound(new { message = "Lien introuvable." });

//            return NoContent();
//        }
//    }
//}
