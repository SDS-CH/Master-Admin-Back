#nullable disable
using Kendo.Mvc.UI;
using Master.DTO.Config;
using Master.Infrastructure.IServices.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Master.API.Controllers.Config
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MasterErpmailTemplateController : GenericController
    {
        private readonly IMasterErpmailTemplateService<MasterErpmailTemplateDTO> _service;

        public MasterErpmailTemplateController(IMasterErpmailTemplateService<MasterErpmailTemplateDTO> service)
        {
            _service = service;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] MasterErpmailTemplateDTO dto)
        {
            var result = await _service.CreateMailTemplate(dto);
            if (result.ErrorOccured) return BadRequest(result.Message);
            return Ok(dto);
        }

        [HttpDelete("{mailId}")]
        public async Task<IActionResult> Delete(int mailId)
        {
            var result = await _service.RemoveMailTemplate(mailId);
            return Ok(mailId);
        }

        [HttpPut("{mailId}")]
        public async Task<IActionResult> Edit(int mailId, [FromBody] MasterErpmailTemplateDTO dto)
        {
            var result = await _service.EditMailTemplate(dto, mailId);
            if (result.ErrorOccured) return BadRequest(result.Message);
            return Ok(dto);
        }

        [HttpGet("{mailId}")]
        public async Task<IActionResult> GetById(int mailId)
            => Ok(await _service.GetById(mailId));

        [HttpPost("read")]
        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAllMailTemplates(request));
    }
}
