using Microsoft.EntityFrameworkCore;
using PayrollSystem.API.Data;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;

namespace PayrollSystem.API.Repositories
{
    public class OvertimeRepository : IOvertimeRepository
    {
        private readonly ApplicationDbContext _context;

        public OvertimeRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OvertimeRequest?> GetByIdAsync(int id)
        {
            return await _context.OvertimeRequests
                .Include(o => o.Employee)
                .Include(o => o.Attendance)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OvertimeRequest>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.OvertimeRequests
                .Include(o => o.Attendance)
                .Where(o => o.EmployeeId == employeeId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<OvertimeRequest>> GetAllAsync()
        {
            return await _context.OvertimeRequests
                .Include(o => o.Employee)
                .Include(o => o.Attendance)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(OvertimeRequest request)
        {
            await _context.OvertimeRequests
                .AddAsync(request);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OvertimeRequest request)
        {
            _context.OvertimeRequests.Update(request);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsForAttendanceAsync(int attendanceId)
        {
            return await _context.OvertimeRequests
                .AnyAsync(o => o.AttendanceId == attendanceId);
        }
    }
}
