using PayrollSystem.API.DTOs.Departments;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(
            IDepartmentRepository repository
            )
        {
            _repository = repository;
        }

        public async Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto request)
        {
            var existingDepartment =
            await _repository.GetByNameAsync(request.Name);

            if (existingDepartment != null)
            {
                throw new InvalidOperationException(
                    "A department with this name already exists.");
            }

            var department = new Department
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(department);

            var departmentDto = new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                EmployeeCount = department.Employees.Count
            };

            return departmentDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
            {
                return false;
            }

            if (department.Employees.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete a department that has employees.");
            }

            await _repository.DeleteAsync(department);

            return true;
        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();
            var departmentDto = departments.Select(dept => new DepartmentResponseDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description,
                IsActive = dept.IsActive,
                CreatedAt = dept.CreatedAt,
                EmployeeCount = dept.Employees.Count
            });

            return departmentDto;
        }

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
            {
                return null;
            }

            var departmentDto = new DepartmentResponseDto
            {
                Id= department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                EmployeeCount= department.Employees.Count
            };

            return departmentDto;
        }

        public async Task<DepartmentResponseDto?> UpdateAsync(int id, UpdateDepartmentDto request)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
            {
                return null;
            }

            var existingDepartment =
                await _repository.GetByNameAsync(request.Name);

            if (existingDepartment != null &&
                existingDepartment.Id != id)
            {
                throw new InvalidOperationException(
                    "A department with this name already exists.");
            }

            department.Name = request.Name.Trim();
            department.Description = request.Description?.Trim();

            await _repository.UpdateAsync(department);

            var departmentDto = new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                EmployeeCount = department.Employees.Count
            };

            return departmentDto;
        }
    }
}
