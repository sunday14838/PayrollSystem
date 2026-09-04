namespace PayrollSystem.API.Models
{
    public class WorkSchedule
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<WorkScheduleDay> Days { get; set; } = new List<WorkScheduleDay>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
