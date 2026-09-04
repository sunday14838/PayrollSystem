using Microsoft.EntityFrameworkCore;
using PayrollSystem.API.Data;
using PayrollSystem.API.DTOs.Attendance;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;

namespace PayrollSystem.API.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);

            await _context.SaveChangesAsync();
        }

        public async Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            var attendanceDate = date.Date;

            return await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.AttendanceDate == attendanceDate);
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IEnumerable<Attendance> Records, int TotalRecords)> GetPagedAsync(AttendanceQueryDto query)
        {
            var records = _context.Attendances
            .Include(a => a.Employee)
            .AsQueryable();

            if (query.EmployeeId.HasValue)
            {
                records = records.Where(a =>
                    a.EmployeeId == query.EmployeeId.Value);
            }

            if (query.StartDate.HasValue)
            {
                var startDate = query.StartDate.Value.Date;

                records = records.Where(a =>
                    a.AttendanceDate >= startDate);
            }

            if (query.EndDate.HasValue)
            {
                var endDate = query.EndDate.Value.Date;

                records = records.Where(a =>
                    a.AttendanceDate <= endDate);
            }

            if (query.Status.HasValue)
            {
                records = records.Where(a =>
                    a.Status == query.Status.Value);
            }

            var totalRecords =
                await records.CountAsync();

            var results = await records
                .OrderByDescending(a => a.AttendanceDate)
                .ThenBy(a => a.Employee.FirstName)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (results, totalRecords);
        }

        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);

            await _context.SaveChangesAsync();
        }
    }
}
