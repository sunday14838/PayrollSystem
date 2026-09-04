using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollSystem.API.DTOs.Overtime;
using PayrollSystem.API.Services.Interfaces;
using System.Security.Claims;

namespace PayrollSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class OvertimeController : ControllerBase
    {
        private readonly IOvertimeService _service;

        public OvertimeController(
            IOvertimeService service)
        {
            _service = service;
        }



        [HttpPost]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create(CreateOvertimeRequestDto request)
        {
            var employeeId = GetEmployeeId();

            var result = await _service.CreateAsync(employeeId, request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpGet("my-requests")]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyRequests()
        {
            var employeeId = GetEmployeeId();

            var result =await _service.GetMyRequestsAsync(employeeId);

            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Overtime request not found."
                });
            }

            return Ok(result);
        }

        [HttpPut("{id:int}/approve")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Approve(int id, ApproveOvertimeDto request)
        {
            var approverId = GetUserId();

            var result = await _service.ApproveAsync(
                    id,
                    approverId,
                    request);

            return Ok(result);
        }

        [HttpPut("{id:int}/reject")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Reject(int id, RejectOvertimeDto request)
        {
            var result = await _service.RejectAsync(id, request);

            return Ok(result);
        }



        private int GetEmployeeId()
        {
            var claim = User.FindFirst("EmployeeId")?.Value;

            if (!int.TryParse(claim, out var employeeId))
            {
                throw new UnauthorizedAccessException(
                    "Employee ID was not found in the authentication token.");
            }

            return employeeId;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(claim, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID was not found in the authentication token.");
            }

            return userId;
        }
    }
}
