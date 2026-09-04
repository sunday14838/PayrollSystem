using PayrollSystem.API.DTOs;
using PayrollSystem.API.DTOs.Employees;

namespace PayrollSystem.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto?> GetByIdAsync(int id);

        Task<PagedResponse<EmployeeResponseDto>> GetPagedAsync(EmployeeQueryDto query);

        Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto request);

        Task<EmployeeResponseDto?> UpdateAsync(int id, UpdateEmployeeDto request);

        Task<bool> DeactivateAsync(int id);

        Task AssignWorkScheduleAsync(int employeeId, int workScheduleId);
    }
}
