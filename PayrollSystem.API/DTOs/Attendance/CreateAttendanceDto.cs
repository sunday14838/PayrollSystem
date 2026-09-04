namespace PayrollSystem.API.DTOs.Attendance
{
    public class CreateAttendanceDto
    {
        public int EmployeeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string? Notes { get; set; }
    }
}
