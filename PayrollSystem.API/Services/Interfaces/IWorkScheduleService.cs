using PayrollSystem.API.DTOs.WorkSchedules;

namespace PayrollSystem.API.Services.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<WorkScheduleResponseDto> CreateAsync(CreateWorkScheduleDto request);

        Task<IEnumerable<WorkScheduleResponseDto>> GetAllAsync();

        Task<WorkScheduleResponseDto?> GetByIdAsync(int id);
    }
}
