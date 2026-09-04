using PayrollSystem.API.Models;

namespace PayrollSystem.API.Repositories.Interfaces
{
    public interface IWorkScheduleRepository
    {
        Task<WorkSchedule?> GetByIdAsync(int id);

        Task<IEnumerable<WorkSchedule>> GetAllAsync();

        Task AddAsync(WorkSchedule schedule);

        Task UpdateAsync(WorkSchedule schedule);

        Task<bool> ExistsAsync(int id);

        Task<WorkSchedule?> GetEmployeeScheduleAsync(int employeeId);
    }
}
