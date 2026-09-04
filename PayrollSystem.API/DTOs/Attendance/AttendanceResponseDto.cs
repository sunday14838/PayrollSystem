namespace PayrollSystem.API.DTOs.Attendance
{
    public class AttendanceResponseDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeNumber { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;

        public DateTime AttendanceDate { get; set; }

        public DateTime? ClockIn { get; set; }

        public DateTime? ClockOut { get; set; }

        public decimal HoursWorked { get; set; }

        public int LateMinutes { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
        public decimal OvertimeHours { get; set; }
    }
}
