using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.API.DTOs.Employees;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,HR,Finance")]
        public async Task<IActionResult> GetAll ([FromQuery] EmployeeQueryDto query)
        {
            var result = await _service.GetPagedAsync(query);

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        //[Authorize(Roles = "Admin,HR,Finance")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee not found."
                });
            }

            return Ok(employee);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create(CreateEmployeeDto request)
        {
            var employee = await _service.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                employee);
        }

        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeDto request)
        {
            var employee = await _service.UpdateAsync(id, request);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee not found."
                });
            }

            return Ok(employee);
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _service.DeactivateAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Employee not found."
                });
            }

            return NoContent();
        }

        [HttpPut("{employeeId:int}/schedule")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> AssignSchedule(int employeeId, AssignWorkScheduleDto request)
        {
            await _service.AssignWorkScheduleAsync(
                employeeId,
                request.WorkScheduleId);

            return Ok(new
            {
                message = "Work schedule assigned successfully."
            });
        }
    }
}
