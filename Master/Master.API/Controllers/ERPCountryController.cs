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

    public class ERPCountryController : GenericController

{

    private readonly IErpCountryService<ErpCountryDTO> _service;



    public ERPCountryController(IErpCountryService<ErpCountryDTO> service)

    {

        _service = service;

    }



    [HttpGet("GetAllCountries")]

    [HttpPost("GetAllCountries")]

    public async Task<IActionResult> GetAll([DataSourceRequest] DataSourceRequest request)

    => Ok(await _service.GetAllCountries(request));



    [HttpPost("GetById/{id}")]

    public async Task<IActionResult> GetById(int id)

    => Ok(await _service.GetById(id));



    [HttpPost("Create")]

    public async Task<IActionResult> Create([FromBody] ErpCountryDTO dto)

    => Ok(await _service.CreateCountry(dto));



    [HttpPost("Edit")]

        public async Task<IActionResult> Edit([FromBody] ErpCountryDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Payload data cannot be null.");
            }

            var result = await _service.EditCountry(dto, dto.Id);

            // If result indicates an error status state, send a server error back to the frontend
            if (result.ErrorOccured) 
            {
                return BadRequest(result.Message); // Correctly issues an explicit 400 Bad Request alert with the error message
            }

            return Ok(result);
        }



        [HttpPut("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(int id, [FromQuery] bool isActive)
        {
            // The controller passes the raw parameters straight to the service layer.
            // No model fetching, no conditional ternary check, no state mutations.
            var result = await _service.ToggleCountryStatus(id, isActive);

            if (result.ErrorOccured)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }



        [HttpPost("Delete/{id}")]

    public async Task<IActionResult> Delete(int id)

    => Ok(await _service.RemoveCountry(id));

}

} 

