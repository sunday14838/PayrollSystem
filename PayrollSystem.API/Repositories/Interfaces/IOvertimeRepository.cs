using PayrollSystem.API.Models;

namespace PayrollSystem.API.Repositories.Interfaces
{
    public interface IOvertimeRepository
    {
        Task<OvertimeRequest?> GetByIdAsync(int id);

        Task<IEnumerable<OvertimeRequest>> GetByEmployeeAsync(int employeeId);

        Task<IEnumerable<OvertimeRequest>> GetAllAsync();

        Task AddAsync(OvertimeRequest request);

        Task UpdateAsync(OvertimeRequest request);

        Task<bool> ExistsForAttendanceAsync(int attendanceId);
    }
}
