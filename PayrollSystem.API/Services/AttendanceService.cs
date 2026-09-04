using PayrollSystem.API.DTOs;
using PayrollSystem.API.DTOs.Attendance;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories;
using PayrollSystem.API.Repositories.Interfaces;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly ITimeService _timeService;

        private static readonly TimeSpan StandardStartTime = new TimeSpan(8, 0, 0);

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IEmployeeRepository employeeRepository,
            IWorkScheduleRepository workScheduleRepository,
            ITimeService timeService)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _workScheduleRepository = workScheduleRepository;
            _timeService = timeService;

        }

        public async Task<AttendanceResponseDto> ClockInAsync(ClockInDto request)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
            {
                throw new InvalidOperationException(
                    "Employee not found.");
            }

            if (!employee.IsActive)
            {
                throw new InvalidOperationException(
                    "Inactive employees cannot clock in.");
            }

            var localNow = _timeService.LocalNow;

            var today = localNow.Date;

            var scheduleDay =
                await GetTodayScheduleAsync(
                    request.EmployeeId,
                    localNow);

            if (scheduleDay == null)
            {
                throw new InvalidOperationException(
                    "Employee does not have a work schedule.");
            }

            if (!scheduleDay.IsWorkingDay)
            {
                throw new InvalidOperationException(
                    "Today is not a scheduled working day.");
            }

            var attendance =
                await _attendanceRepository
                    .GetByEmployeeAndDateAsync(
                        request.EmployeeId,
                        today);

            if (attendance != null && attendance.ClockIn.HasValue)
            {
                throw new InvalidOperationException(
                    "Employee has already clocked in today.");
            }

            var lateMinutes =
                CalculateLateMinutes(
                    localNow.TimeOfDay,
                    scheduleDay.StartTime!.Value);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    EmployeeId = request.EmployeeId,

                    AttendanceDate = today,

                    ClockIn = localNow,

                    Status = lateMinutes > 0
                        ? AttendanceStatus.Late
                        : AttendanceStatus.Present,

                    LateMinutes = lateMinutes,

                    CreatedAt = _timeService.UtcNow
                };

                await _attendanceRepository
                    .AddAsync(attendance);
            }
            else
            {
                if (attendance.ClockIn.HasValue)
                {
                    throw new InvalidOperationException(
                        "Employee has already clocked in today.");
                }

                attendance.ClockIn = localNow;

                attendance.Status = lateMinutes > 0
                    ? AttendanceStatus.Late
                    : AttendanceStatus.Present;

                attendance.LateMinutes = lateMinutes;

                attendance.UpdatedAt = _timeService.UtcNow;

                await _attendanceRepository
                    .UpdateAsync(attendance);
            }

            var created =
                await _attendanceRepository
                    .GetByIdAsync(attendance.Id);

            return MapToResponse(created!);
        }

        public async Task<AttendanceResponseDto> ClockOutAsync(ClockOutDto request)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
            {
                throw new InvalidOperationException(
                    "Employee not found.");
            }

            if (!employee.IsActive)
            {
                throw new InvalidOperationException(
                    "Inactive employees cannot clock out.");
            }

            var localNow = _timeService.LocalNow;

            var attendance =
                await _attendanceRepository
                    .GetByEmployeeAndDateAsync(
                        request.EmployeeId,
                        localNow.Date);

            if (attendance == null)
            {
                throw new InvalidOperationException(
                    "No attendance record exists for today.");
            }

            if (!attendance.ClockIn.HasValue)
            {
                throw new InvalidOperationException(
                    "Employee has not clocked in.");
            }

            if (attendance.ClockOut.HasValue)
            {
                throw new InvalidOperationException(
                    "Employee has already clocked out today.");
            }

            var scheduleDay =
                await GetTodayScheduleAsync(
                    request.EmployeeId,
                    localNow);

            if (scheduleDay == null ||
                !scheduleDay.IsWorkingDay)
            {
                throw new InvalidOperationException(
                    "Employee does not have a valid work schedule for today.");
            }

            attendance.ClockOut = localNow;

            var totalDuration =
                localNow - attendance.ClockIn.Value;

            var breakDuration =
                scheduleDay.BreakDuration
                ?? TimeSpan.Zero;

            var workedDuration =
                totalDuration - breakDuration;

            if (workedDuration < TimeSpan.Zero)
            {
                workedDuration = TimeSpan.Zero;
            }

            var scheduledDuration =
                scheduleDay.EndTime!.Value -
                scheduleDay.StartTime!.Value -
                breakDuration;

            if (scheduledDuration < TimeSpan.Zero)
            {
                scheduledDuration = TimeSpan.Zero;
            }

            var regularDuration =
                workedDuration <= scheduledDuration
                    ? workedDuration
                    : scheduledDuration;

            var overtimeDuration =
                workedDuration > scheduledDuration
                    ? workedDuration - scheduledDuration
                    : TimeSpan.Zero;


            attendance.HoursWorked =
                Math.Round(
                    (decimal)regularDuration.TotalHours,
                    2);

            attendance.OvertimeHours =
                Math.Round(
                    (decimal)overtimeDuration.TotalHours,
                    2);

            if (attendance.HoursWorked < 4)
            {
                attendance.Status =
                    AttendanceStatus.HalfDay;
            }

            attendance.UpdatedAt =
                _timeService.UtcNow;

            await _attendanceRepository
                .UpdateAsync(attendance);

            return MapToResponse(attendance);
        }

        public async Task<AttendanceResponseDto> CreateManualAsync(CreateAttendanceDto request)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
            {
                throw new InvalidOperationException(
                    "Employee not found.");
            }

            if (!employee.IsActive)
            {
                throw new InvalidOperationException(
                    "Attendance cannot be created for an inactive employee.");
            }

            var attendanceDate = request.AttendanceDate.Date;

            var existing = await _attendanceRepository.GetByEmployeeAndDateAsync(
                        request.EmployeeId,
                        attendanceDate);

            if (existing != null)
            {
                throw new InvalidOperationException(
                    "Attendance already exists for this employee and date.");
            }

            var attendance = new Attendance
            {
                EmployeeId = request.EmployeeId,

                AttendanceDate = attendanceDate,

                Status = AttendanceStatus.Absent,

                Notes = request.Notes?.Trim(),

                CreatedAt = DateTime.UtcNow
            };

            await _attendanceRepository.AddAsync(attendance);

            var created = await _attendanceRepository.GetByIdAsync(attendance.Id);

            return MapToResponse(created!);
        }

        public async Task<AttendanceResponseDto?> GetByIdAsync(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);

            if (attendance == null)
            {
                return null;
            }

            return MapToResponse(attendance);
        }

        public async Task<PagedResponse<AttendanceResponseDto>> GetPagedAsync(AttendanceQueryDto query)
        {
            if (query.PageNumber < 1)
            {
                query.PageNumber = 1;
            }

            if (query.PageSize < 1)
            {
                query.PageSize = 20;
            }

            if (query.PageSize > 100)
            {
                query.PageSize = 100;
            }

            var result = await _attendanceRepository
                    .GetPagedAsync(query);

            return new PagedResponse<AttendanceResponseDto>
            {
                Data = result.Records.Select(MapToResponse),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = result.TotalRecords
            };
        }






        private async Task<WorkScheduleDay?> GetTodayScheduleAsync(
            int employeeId,
            DateTime localDate)
        {
            var schedule = await _workScheduleRepository.GetEmployeeScheduleAsync(employeeId);

            if (schedule == null)
            {
                return null;
            }

            return schedule.Days.FirstOrDefault(d => d.DayOfWeek == localDate.DayOfWeek);
        }

        private static int CalculateLateMinutes(TimeSpan actualTime, TimeSpan expectedTime)
        {
            if (actualTime <= expectedTime)
            {
                return 0;
            }

            return (int)(
                actualTime - expectedTime
            ).TotalMinutes;
        }

        private static AttendanceResponseDto MapToResponse(Attendance attendance)
        {
            return new AttendanceResponseDto
            {
                Id = attendance.Id,

                EmployeeId = attendance.EmployeeId,

                EmployeeNumber =
                    attendance.Employee.EmployeeNumber,

                EmployeeName =
                    $"{attendance.Employee.FirstName} " +
                    $"{attendance.Employee.LastName}",

                AttendanceDate =
                    attendance.AttendanceDate,

                ClockIn =
                    attendance.ClockIn,

                ClockOut =
                    attendance.ClockOut,

                HoursWorked =
                    attendance.HoursWorked,

                LateMinutes =
                    attendance.LateMinutes,
                OvertimeHours =
                    attendance.OvertimeHours,

                Status =
                    attendance.Status.ToString(),

                Notes =
                    attendance.Notes,
                
            };
        }
    }
}
