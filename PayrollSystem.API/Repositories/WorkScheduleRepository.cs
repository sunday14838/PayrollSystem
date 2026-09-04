using Microsoft.EntityFrameworkCore;
using PayrollSystem.API.Data;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;

namespace PayrollSystem.API.Repositories
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkScheduleRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkSchedule?> GetByIdAsync(int id)
        {
            return await _context.WorkSchedules
                .Include(w => w.Days)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<WorkSchedule>> GetAllAsync()
        {
            return await _context.WorkSchedules
                .Include(w => w.Days)
                .ToListAsync();
        }

        public async Task AddAsync(WorkSchedule schedule)
        {
            await _context.WorkSchedules.AddAsync(schedule);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkSchedule schedule)
        {
            _context.WorkSchedules.Update(schedule);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.WorkSchedules
                .AnyAsync(w => w.Id == id);
        }

        public async Task<WorkSchedule?> GetEmployeeScheduleAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.WorkSchedule)
                    .ThenInclude(w => w!.Days)
                .Where(e => e.Id == employeeId)
                .Select(e => e.WorkSchedule)
                //.Include(w => w!.Days)
                .FirstOrDefaultAsync();
        }
    }
}
