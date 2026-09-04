namespace PayrollSystem.API.Models
{
    public class WorkScheduleDay
    {
        public int Id { get; set; }

        public int WorkScheduleId { get; set; }

        public WorkSchedule WorkSchedule { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; }

        public bool IsWorkingDay { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public TimeSpan? BreakDuration { get; set; }
    }
}
