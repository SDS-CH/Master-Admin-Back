#nullable disable
using Kendo.Mvc.UI;
using Master.DTO.Config;
using Master.Infrastructure.IServices.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Master.API.Controllers.Config
{
    [Route("MasterErpglobalConfigs")]
    [ApiController]
    [Authorize]
    public class MasterErpglobalConfigController : GenericController
    {
        private readonly IMasterErpglobalConfigService<MasterErpglobalConfigDTO> _service;

        public MasterErpglobalConfigController(IMasterErpglobalConfigService<MasterErpglobalConfigDTO> service)
        {
            _service = service;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] MasterErpglobalConfigDTO dto)
        {
            var result = await _service.CreateConfig(dto);
            if (result.ErrorOccured) return BadRequest(result.Message);
            return Ok(dto);
        }

        [HttpDelete("{globalConfigId}")]
        public IActionResult Delete(int globalConfigId)
            => Ok(globalConfigId);

        [HttpPut("{globalConfigId}")]
        public async Task<IActionResult> Edit(int globalConfigId, [FromBody] MasterErpglobalConfigDTO dto)
        {
            var result = await _service.EditConfig(dto, globalConfigId);
            if (result.ErrorOccured) return BadRequest(result.Message);
            return Ok(await _service.GetById(globalConfigId));
        }

        [HttpGet("{globalConfigId}")]
        public async Task<IActionResult> GetByID(int globalConfigId)
            => Ok(await _service.GetById(globalConfigId));

        [HttpGet("GetCurrentConfig")]
        public async Task<IActionResult> GetCurrentConfig()
            => Ok(await _service.GetFirstConfig());

        [HttpPost("read")]
        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
            => Ok(await _service.GetAll(request));

        [HttpGet("/MasterErpglobalConfig/{environmentName}")]
        public async Task<IActionResult> GetByEnvironmentName(string environmentName)
            => Ok(await _service.GetFirstConfig());

        [HttpGet("/MasterErpglobalConfig/GetTemporaryPhysicalPath")]
        public IActionResult GetTemporaryPhysicalPath()
            => Ok(string.Empty);
    }
}
