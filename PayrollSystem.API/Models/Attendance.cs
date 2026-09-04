namespace PayrollSystem.API.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public DateTime AttendanceDate { get; set; }

        public DateTime? ClockIn { get; set; }

        public DateTime? ClockOut { get; set; }

        public decimal HoursWorked { get; set; }

        public int LateMinutes { get; set; }

        public AttendanceStatus Status { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public decimal OvertimeHours { get; set; }
    }
}
