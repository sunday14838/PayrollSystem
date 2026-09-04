namespace PayrollSystem.API.Models
{
    public class OvertimeRequest
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;

        public int AttendanceId { get; set; }

        public Attendance Attendance { get; set; } = null!;

        public decimal RequestedHours { get; set; }

        public decimal ApprovedHours { get; set; }

        public string Reason { get; set; } = string.Empty;

        public OvertimeStatus Status { get; set; } = OvertimeStatus.Pending;

        public string? RejectionReason { get; set; }

        public int? ApprovedById { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
