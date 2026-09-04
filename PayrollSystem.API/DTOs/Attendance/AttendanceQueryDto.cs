using PayrollSystem.API.Models;

namespace PayrollSystem.API.DTOs.Attendance
{
    public class AttendanceQueryDto
    {
        public int? EmployeeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public AttendanceStatus? Status { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
