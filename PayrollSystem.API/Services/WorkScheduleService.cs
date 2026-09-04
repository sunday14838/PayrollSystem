using PayrollSystem.API.DTOs.WorkSchedules;
using PayrollSystem.API.Models;
using PayrollSystem.API.Repositories.Interfaces;
using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly IWorkScheduleRepository _repository;

        public WorkScheduleService(
            IWorkScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<WorkScheduleResponseDto> CreateAsync(CreateWorkScheduleDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new InvalidOperationException(
                    "Work schedule name is required.");
            }

            if (request.Days.Count != 7)
            {
                throw new InvalidOperationException(
                    "A work schedule must contain all seven days.");
            }

            var duplicateDays = request.Days
                .GroupBy(d => d.DayOfWeek)
                .Any(g => g.Count() > 1);

            if (duplicateDays)
            {
                throw new InvalidOperationException(
                    "Each day of the week can only appear once.");
            }

            foreach (var day in request.Days)
            {
                if (day.IsWorkingDay)
                {
                    if (!day.StartTime.HasValue ||
                        !day.EndTime.HasValue)
                    {
                        throw new InvalidOperationException(
                            $"{day.DayOfWeek} requires a start and end time.");
                    }

                    if (day.EndTime <= day.StartTime)
                    {
                        throw new InvalidOperationException(
                            $"{day.DayOfWeek} end time must be after start time.");
                    }
                }
            }

            var schedule = new WorkSchedule
            {
                Name = request.Name.Trim(),

                Description =
                    request.Description?.Trim(),

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            };

            foreach (var day in request.Days)
            {
                schedule.Days.Add(new WorkScheduleDay
                {
                    DayOfWeek = day.DayOfWeek,

                    IsWorkingDay = day.IsWorkingDay,

                    StartTime = day.StartTime,

                    EndTime = day.EndTime,

                    BreakDuration = day.BreakDuration
                });
            }

            await _repository.AddAsync(schedule);

            return MapToResponse(schedule);
        }

        public async Task<IEnumerable<WorkScheduleResponseDto>> GetAllAsync()
        {
            var schedules =
                await _repository.GetAllAsync();

            return schedules.Select(MapToResponse);
        }

        public async Task<WorkScheduleResponseDto?> GetByIdAsync(
            int id)
        {
            var schedule =
                await _repository.GetByIdAsync(id);

            if (schedule == null)
            {
                return null;
            }

            return MapToResponse(schedule);
        }






        private static WorkScheduleResponseDto MapToResponse(
            WorkSchedule schedule)
        {
            return new WorkScheduleResponseDto
            {
                Id = schedule.Id,

                Name = schedule.Name,

                Description = schedule.Description,

                IsActive = schedule.IsActive,

                Days = schedule.Days
                    .OrderBy(d => d.DayOfWeek)
                    .Select(d => new WorkScheduleDayDto
                    {
                        DayOfWeek = d.DayOfWeek,

                        IsWorkingDay = d.IsWorkingDay,

                        StartTime = d.StartTime,

                        EndTime = d.EndTime,

                        BreakDuration = d.BreakDuration
                    })
                    .ToList()
            };
        }
    }
}
