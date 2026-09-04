using PayrollSystem.API.DTOs;
using PayrollSystem.API.DTOs.Attendance;

namespace PayrollSystem.API.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDto?> GetByIdAsync(int id);

        Task<PagedResponse<AttendanceResponseDto>> GetPagedAsync(AttendanceQueryDto query);

        Task<AttendanceResponseDto> CreateManualAsync(CreateAttendanceDto request);

        Task<AttendanceResponseDto> ClockInAsync(ClockInDto request);

        Task<AttendanceResponseDto> ClockOutAsync(ClockOutDto request);
    }
}
