using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.API.DTOs.WorkSchedules;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WorkSchedulesController : ControllerBase
    {
        private readonly IWorkScheduleService _service;

        public WorkSchedulesController(IWorkScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _service.GetAllAsync();

            return Ok(schedules);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _service.GetByIdAsync(id);

            if (schedule == null)
            {
                return NotFound(new
                {
                    message = "Work schedule not found."
                });
            }

            return Ok(schedule);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create(CreateWorkScheduleDto request)
        {
            var schedule = await _service.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = schedule.Id },
                schedule);
        }
    }
}
