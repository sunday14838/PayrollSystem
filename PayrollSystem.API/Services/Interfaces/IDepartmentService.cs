using PayrollSystem.API.DTOs.Departments;

namespace PayrollSystem.API.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync();

        Task<DepartmentResponseDto?> GetByIdAsync(int id);

        Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto request);

        Task<DepartmentResponseDto?> UpdateAsync(int id, UpdateDepartmentDto request);

        Task<bool> DeleteAsync(int id);
    }
}
