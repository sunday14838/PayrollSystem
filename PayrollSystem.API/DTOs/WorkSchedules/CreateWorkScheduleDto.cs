namespace PayrollSystem.API.DTOs.WorkSchedules
{
    public class CreateWorkScheduleDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<WorkScheduleDayDto> Days { get; set; } = new();
    }
}
