using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using DMS.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class ActivitiesController : ControllerBase

    {

        private readonly IActivityService<ActivityDto> _service;

        public ActivitiesController(IActivityService<ActivityDto> service)

        {

            _service = service;

        }



        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] int? industryId, [FromQuery] bool unassignedOnly = false)

        {

            var activities = await _service.GetAll(industryId, unassignedOnly);

            return Ok(activities);

        }

        // POST api/activities

        [HttpPost]

        public async Task<IActionResult> Add(ActivityDto dto)

        {

            await _service.Add(dto);

            return Ok(new { message = "Activité ajoutée !" });

        }

        [HttpPut("{codeActivite}")]

        public async Task<IActionResult> Update(string codeActivite, [FromQuery] Guid? tenantId, ActivityDto dto)

        {

            var updated = await _service.Update(codeActivite, tenantId ?? Guid.Empty, dto);

            return updated ? Ok(new { message = "Activité modifiée !" }) : NotFound();

        }

        [HttpDelete("{codeActivite}")]

        public async Task<IActionResult> Delete(string codeActivite, [FromQuery] Guid? tenantId)

        {

            var deleted = await _service.Delete(codeActivite, tenantId ?? Guid.Empty);

            return deleted ? Ok(new { message = "Activité supprimée !" }) : NotFound();

        }

        [HttpDelete]

        public async Task<IActionResult> DeleteByQuery([FromQuery] string codeActivite, [FromQuery] Guid? tenantId)

        {

            var deleted = await _service.Delete(codeActivite ?? string.Empty, tenantId ?? Guid.Empty);

            return deleted ? Ok(new { message = "Activité supprimée !" }) : NotFound();

        }

    }

}
