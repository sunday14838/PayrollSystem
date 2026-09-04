using Microsoft.EntityFrameworkCore;
using PayrollSystem.API.Data;
using PayrollSystem.API.DTOs.Employees;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;

namespace PayrollSystem.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeEmployeeId = null)
        {
            var query =  _context.Employees.Where(e => e.Email.ToLower() == email.ToLower());

            if (excludeEmployeeId.HasValue)
            {
                query = query.Where(e=> e.Id != excludeEmployeeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
             return await _context.Employees.FirstOrDefaultAsync(e=> e.Email.ToLower() == email.ToLower());
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
           return await _context.Employees
                .Include(e=> e.Department)
                .Include(e => e.WorkSchedule)
                .FirstOrDefaultAsync(e=> e.Id == id);
        }

        public async Task<Employee?> GetByIdWithScheduleAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.WorkSchedule)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> GetNextEmployeeNumberAsync()
        {
            var lastEmployeeNumber =
            await _context.Employees
                .OrderByDescending(e => e.Id)
                .Select(e => e.EmployeeNumber)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastEmployeeNumber))
            {
                return 1;
            }

            var numberPart = lastEmployeeNumber
                .Replace("EMP-", "");

            return int.Parse(numberPart) + 1;
        }

        public async Task<(IEnumerable<Employee> Employees, int TotalRecords)> GetPagedAsync(EmployeeQueryDto query)
        {
            var employees = _context.Employees
                .Include(e => e.WorkSchedule)
                .Include(e => e.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();

                employees = employees.Where(e =>
                    e.FirstName.ToLower().Contains(search) ||
                    e.LastName.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.EmployeeNumber.ToLower().Contains(search) ||
                    e.JobTitle.ToLower().Contains(search));
            }

            if (query.DepartmentId.HasValue)
            {
                employees = employees.Where(e =>
                    e.DepartmentId == query.DepartmentId.Value);
            }

            if (query.IsActive.HasValue)
            {
                employees = employees.Where(e =>
                    e.IsActive == query.IsActive.Value);
            }

            var totalRecords = await employees.CountAsync();

            var results = await employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (results, totalRecords);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
