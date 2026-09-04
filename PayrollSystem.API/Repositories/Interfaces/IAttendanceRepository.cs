using PayrollSystem.API.DTOs.Attendance;
using PayrollSystem.API.Models;

namespace PayrollSystem.API.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByIdAsync(int id);

        Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);

        Task<(IEnumerable<Attendance> Records, int TotalRecords)> GetPagedAsync(AttendanceQueryDto query);

        Task AddAsync(Attendance attendance);

        Task UpdateAsync(Attendance attendance);
    }
}
