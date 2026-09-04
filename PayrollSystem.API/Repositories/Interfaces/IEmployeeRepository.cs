using PayrollSystem.API.DTOs.Employees;
using PayrollSystem.API.Models;

namespace PayrollSystem.API.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(int id);

        Task<Employee?> GetByEmailAsync(string email);

        Task<bool> ExistsByEmailAsync(string email, int? excludeEmployeeId = null);

        Task<int> GetNextEmployeeNumberAsync();

        Task<(IEnumerable<Employee> Employees, int TotalRecords)> GetPagedAsync(EmployeeQueryDto query);

        Task AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);
        Task<Employee?> GetByIdWithScheduleAsync(int id);
    }
}
