#nullable disable
using Kendo.Mvc.UI;
using Master.DTO.DTOs;
using Master.Infrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Master.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GedDirectoryController : GenericController
    {
        private readonly IGedDirectoryService<GedDirectoryDTO> _service;

        public GedDirectoryController(IGedDirectoryService<GedDirectoryDTO> service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllGedDirectories(request));

        [HttpPost("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GedDirectoryDTO dto)
            => Ok(await _service.CreateGedDirectory(dto));

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromBody] GedDirectoryDTO dto)
            => Ok(await _service.EditGedDirectory(dto, dto.Id));

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.RemoveGedDirectory(id));
    }
}
