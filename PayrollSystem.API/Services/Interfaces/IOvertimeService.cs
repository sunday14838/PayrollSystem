using PayrollSystem.API.DTOs.Overtime;

namespace PayrollSystem.API.Services.Interfaces
{
    public interface IOvertimeService
    {
        Task<OvertimeResponseDto> CreateAsync(int employeeId, CreateOvertimeRequestDto request);

        Task<IEnumerable<OvertimeResponseDto>> GetMyRequestsAsync(int employeeId);

        Task<IEnumerable<OvertimeResponseDto>> GetAllAsync();

        Task<OvertimeResponseDto?> GetByIdAsync(int id);

        Task<OvertimeResponseDto> ApproveAsync(int id, int approverId, ApproveOvertimeDto request);

        Task<OvertimeResponseDto> RejectAsync(int id, RejectOvertimeDto request);
    }
}
