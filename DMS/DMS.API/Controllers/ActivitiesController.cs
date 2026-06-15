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

        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service)

        {

            _service = service;

        }

        // GET api/activities

        [HttpGet]

        public async Task<IActionResult> GetAll()

        {

            var activities = await _service.GetAll();

            return Ok(activities);

        }

        // POST api/activities

        [HttpPost]

        public async Task<IActionResult> Add(ActivityDto dto)

        {

            await _service.Add(dto);

            return Ok(new { message = "Activité ajoutée !" });

        }

    }

}
