namespace PayrollSystem.API.DTOs.Overtime
{
    public class OvertimeResponseDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int AttendanceId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public decimal RequestedHours { get; set; }

        public decimal ApprovedHours { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public int? ApprovedById { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
