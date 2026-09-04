using Microsoft.EntityFrameworkCore;
using PayrollSystem.API.Data;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;

namespace PayrollSystem.API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments
            .AnyAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
           return await _context.Departments
                .Include(d =>d.Employees)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
             .Include(d => d.Employees)
             .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _context.Departments
            .FirstOrDefaultAsync(d =>
                d.Name.ToLower() == name.ToLower());
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);

            await _context.SaveChangesAsync();
        }
    }
}
