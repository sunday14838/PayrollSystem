namespace PayrollSystem.API.DTOs.WorkSchedules
{
    public class WorkScheduleResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public List<WorkScheduleDayDto> Days { get; set; } = new();
    }
}
