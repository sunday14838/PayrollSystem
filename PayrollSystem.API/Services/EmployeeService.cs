using AutoMapper;
using PayrollSystem.API.DTOs;
using PayrollSystem.API.DTOs.Employees;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories;
using PayrollSystem.API.Repositories.Interfaces;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IWorkScheduleRepository workScheduleRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _workScheduleRepository = workScheduleRepository;
            _mapper = mapper;
        }


        public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto request)
        {
            var department =
            await _departmentRepository
                .GetByIdAsync(request.DepartmentId);

            if (department == null)
            {
                throw new InvalidOperationException(
                    "The selected department does not exist.");
            }

            if (!department.IsActive)
            {
                throw new InvalidOperationException(
                    "Employees cannot be assigned to an inactive department.");
            }

            var emailExists =
                await _employeeRepository
                    .ExistsByEmailAsync(request.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "An employee with this email already exists.");
            }

            var employeeNumber =
                await _employeeRepository
                    .GetNextEmployeeNumberAsync();

            var employee = new Employee
            {
                EmployeeNumber =
                    $"EMP-{employeeNumber:D6}",

                FirstName = request.FirstName.Trim(),

                LastName = request.LastName.Trim(),

                Email = request.Email.Trim().ToLower(),

                PhoneNumber = request.PhoneNumber?.Trim(),

                JobTitle = request.JobTitle.Trim(),

                HireDate = request.HireDate,

                BasicSalary = request.BasicSalary,

                DepartmentId = request.DepartmentId,

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            };

            await _employeeRepository.AddAsync(employee);

            var createdEmployee =
                await _employeeRepository
                    .GetByIdAsync(employee.Id);

            var mapper = _mapper.Map<EmployeeResponseDto>(createdEmployee!);
            return mapper;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return false;
            }

            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);

            return true;
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return null;
            }

            var mapper = _mapper.Map<EmployeeResponseDto>(employee);
            return mapper;
        }

        public async Task<PagedResponse<EmployeeResponseDto>> GetPagedAsync(EmployeeQueryDto query)
        {
            if (query.PageNumber < 1)
            {
                query.PageNumber = 1;
            }

            if (query.PageSize < 1)
            {
                query.PageSize = 10;
            }

            if (query.PageSize > 100)
            {
                query.PageSize = 100;
            }

            var result =
                await _employeeRepository.GetPagedAsync(query);

            var employees = result.Employees
                .Select(emp => _mapper.Map<EmployeeResponseDto>(emp))
                .ToList();

            return new PagedResponse<EmployeeResponseDto>
            {
                Data = employees,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<EmployeeResponseDto?> UpdateAsync(int id, UpdateEmployeeDto request)
        {
            var employee =
            await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return null;
            }

            var department =
                await _departmentRepository
                    .GetByIdAsync(request.DepartmentId);

            if (department == null)
            {
                throw new InvalidOperationException(
                    "The selected department does not exist.");
            }

            if (!department.IsActive)
            {
                throw new InvalidOperationException(
                    "Employees cannot be assigned to an inactive department.");
            }

            var emailExists =
                await _employeeRepository
                    .ExistsByEmailAsync(
                        request.Email,
                        id);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Another employee already uses this email.");
            }

            employee.FirstName = request.FirstName.Trim();

            employee.LastName = request.LastName.Trim();

            employee.Email = request.Email.Trim().ToLower();

            employee.PhoneNumber = request.PhoneNumber?.Trim();

            employee.JobTitle = request.JobTitle.Trim();

            employee.HireDate = request.HireDate;

            employee.BasicSalary = request.BasicSalary;

            employee.DepartmentId = request.DepartmentId;

            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);

            var mapper = _mapper.Map<EmployeeResponseDto>(employee);
            return mapper;
        }

        public async Task AssignWorkScheduleAsync(int employeeId, int workScheduleId)
        {
            var employee = await _employeeRepository.GetByIdWithScheduleAsync(employeeId);

            if (employee == null)
            {
                throw new InvalidOperationException(
                    "Employee not found.");
            }

            if (!employee.IsActive)
            {
                throw new InvalidOperationException(
                    "Inactive employees cannot be assigned a work schedule.");
            }

            var schedule = await _workScheduleRepository.GetByIdAsync(workScheduleId);

            if (schedule == null)
            {
                throw new InvalidOperationException(
                    "Work schedule not found.");
            }

            if (!schedule.IsActive)
            {
                throw new InvalidOperationException(
                    "Cannot assign an inactive work schedule.");
            }

            employee.WorkScheduleId = workScheduleId;

            employee.WorkSchedule = schedule;

            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);
        }





        //private static EmployeeResponseDto MapToResponse(
        //Employee employee)
        //{
        //    return new EmployeeResponseDto
        //    {
        //        Id = employee.Id,

        //        EmployeeNumber =
        //            employee.EmployeeNumber,

        //        FirstName =
        //            employee.FirstName,

        //        LastName =
        //            employee.LastName,

        //        Email =
        //            employee.Email,

        //        PhoneNumber =
        //            employee.PhoneNumber,

        //        JobTitle =
        //            employee.JobTitle,

        //        HireDate =
        //            employee.HireDate,

        //        BasicSalary =
        //            employee.BasicSalary,

        //        IsActive =
        //            employee.IsActive,

        //        DepartmentId =
        //            employee.DepartmentId,

        //        DepartmentName =
        //            employee.Department.Name,

        //        CreatedAt =
        //            employee.CreatedAt,

        //        UpdatedAt =
        //            employee.UpdatedAt,
        //        WorkScheduleId = employee.WorkScheduleId,

        //        WorkScheduleName = employee.WorkSchedule?.Name,
        //    };
        //}

        
    }
}
