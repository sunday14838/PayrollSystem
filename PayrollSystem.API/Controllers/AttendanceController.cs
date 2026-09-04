using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.API.DTOs.Attendance;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _service;

        public AttendanceController(
            IAttendanceService service)
        {
            _service = service;
        }


        [HttpGet]
        //[Authorize(Roles = "Admin,HR,Finance")]
        public async Task<IActionResult> GetAll([FromQuery] AttendanceQueryDto query)
        {
            var result = await _service.GetPagedAsync(query);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "Admin,HR,Finance")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _service.GetByIdAsync(id);

            if (attendance == null)
            {
                return NotFound(new
                {
                    message = "Attendance record not found."
                });
            }

            return Ok(attendance);
        }

        [HttpPost("hr create manually")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create(CreateAttendanceDto request)
        {
            var attendance = await _service.CreateManualAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = attendance.Id },
                attendance);
        }

        [HttpPost("clock-in")]
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> ClockIn(ClockInDto request)
        {
            var attendance = await _service.ClockInAsync(request);

            return Ok(attendance);
        }

        [HttpPost("clock-out")]
        //[Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> ClockOut(ClockOutDto request)
        {
            var attendance = await _service.ClockOutAsync(request);

            return Ok(attendance);
        }
    }
}
