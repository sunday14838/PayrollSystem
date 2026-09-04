namespace PayrollSystem.API.DTOs.WorkSchedules
{
    public class WorkScheduleDayDto
    {
        public DayOfWeek DayOfWeek { get; set; }

        public bool IsWorkingDay { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakDuration { get; set; }
    }
}
