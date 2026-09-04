using AutoMapper;
using PayrollSystem.API.DTOs.Overtime;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class OvertimeService : IOvertimeService
    {
        private readonly IOvertimeRepository _repository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ITimeService _timeService;
        private readonly IMapper _mapper;

        public OvertimeService(
            IOvertimeRepository repository,
            IAttendanceRepository attendanceRepository,
            ITimeService timeService,
            IMapper mapper)
        {
            _repository = repository;
            _attendanceRepository = attendanceRepository;
            _timeService = timeService;
            _mapper = mapper;
        }

        public async Task<OvertimeResponseDto> CreateAsync(
            int employeeId,
            CreateOvertimeRequestDto request)
        {
            if (request.Hours <= 0)
            {
                throw new InvalidOperationException(
                    "Overtime hours must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                throw new InvalidOperationException(
                    "A reason is required for overtime.");
            }

            var attendance =
                await _attendanceRepository
                    .GetByIdAsync(request.AttendanceId);

            if (attendance == null)
            {
                throw new InvalidOperationException(
                    "Attendance record not found.");
            }

            if (attendance.EmployeeId != employeeId)
            {
                throw new InvalidOperationException(
                    "This attendance record does not belong to the employee.");
            }

            if (!attendance.ClockIn.HasValue ||
                !attendance.ClockOut.HasValue)
            {
                throw new InvalidOperationException(
                    "Attendance must have both clock-in and clock-out times.");
            }

            if (attendance.OvertimeHours <= 0)
            {
                throw new InvalidOperationException(
                    "No overtime was detected for this attendance record.");
            }

            if (request.Hours > attendance.OvertimeHours)
            {
                throw new InvalidOperationException(
                    $"Requested overtime cannot exceed detected overtime of " +
                    $"{attendance.OvertimeHours} hours.");
            }

            var alreadyExists =
                await _repository
                    .ExistsForAttendanceAsync(
                        request.AttendanceId);

            if (alreadyExists)
            {
                throw new InvalidOperationException(
                    "An overtime request already exists for this attendance record.");
            }

            var overtime = new OvertimeRequest
            {
                EmployeeId = employeeId,

                AttendanceId = request.AttendanceId,

                RequestedHours =
                    Math.Round(request.Hours, 2),

                ApprovedHours = 0,

                Reason = request.Reason.Trim(),

                Status = OvertimeStatus.Pending,

                CreatedAt = _timeService.UtcNow
            };

            await _repository.AddAsync(overtime);

            var mapper = _mapper.Map<OvertimeResponseDto>(overtime);

            return mapper;
        }

        public async Task<IEnumerable<OvertimeResponseDto>>
            GetMyRequestsAsync(int employeeId)
        {
            var requests =
                await _repository
                    .GetByEmployeeAsync(employeeId);

            var mapper = _mapper.Map<IEnumerable<OvertimeResponseDto>>(requests);

            return mapper;
        }

        public async Task<IEnumerable<OvertimeResponseDto>>
            GetAllAsync()
        {
            var requests =
                await _repository.GetAllAsync();

            var mapper = _mapper.Map<IEnumerable<OvertimeResponseDto>>(requests);

            return mapper;
        }

        public async Task<OvertimeResponseDto?> GetByIdAsync(
            int id)
        {
            var request =
                await _repository.GetByIdAsync(id);


            return request == null
                ? null
                : _mapper.Map<OvertimeResponseDto>(request);
        }

        public async Task<OvertimeResponseDto> ApproveAsync(
            int id,
            int approverId,
            ApproveOvertimeDto request)
        {
            var overtime =
                await _repository.GetByIdAsync(id);

            if (overtime == null)
            {
                throw new InvalidOperationException(
                    "Overtime request not found.");
            }

            if (overtime.Status != OvertimeStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending overtime requests can be approved.");
            }

            var approvedHours =
                request.ApprovedHours
                ?? overtime.RequestedHours;

            if (approvedHours <= 0)
            {
                throw new InvalidOperationException(
                    "Approved overtime must be greater than zero.");
            }

            if (approvedHours > overtime.RequestedHours)
            {
                throw new InvalidOperationException(
                    "Approved hours cannot exceed requested hours.");
            }

            overtime.ApprovedHours =
                Math.Round(approvedHours, 2);

            overtime.Status =
                OvertimeStatus.Approved;

            overtime.ApprovedById =
                approverId;

            overtime.ApprovedAt =
                _timeService.UtcNow;

            overtime.UpdatedAt =
                _timeService.UtcNow;

            await _repository.UpdateAsync(overtime);

            var mapper = _mapper.Map<OvertimeResponseDto>(overtime);

            return mapper;
        }

        public async Task<OvertimeResponseDto> RejectAsync(
            int id,
            RejectOvertimeDto request)
        {
            var overtime =
                await _repository.GetByIdAsync(id);

            if (overtime == null)
            {
                throw new InvalidOperationException(
                    "Overtime request not found.");
            }

            if (overtime.Status != OvertimeStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending overtime requests can be rejected.");
            }

            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                throw new InvalidOperationException(
                    "A rejection reason is required.");
            }

            overtime.Status =
                OvertimeStatus.Rejected;

            overtime.ApprovedHours = 0;

            overtime.RejectionReason =
                request.Reason.Trim();

            overtime.UpdatedAt =
                _timeService.UtcNow;

            await _repository.UpdateAsync(overtime);

            var mapper = _mapper.Map<OvertimeResponseDto>(overtime);

            return mapper;
        }
    }
}
