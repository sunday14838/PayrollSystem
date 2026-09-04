namespace PayrollSystem.API.DTOs.Overtime
{
    public class CreateOvertimeRequestDto
    {
        public int AttendanceId { get; set; }

        public decimal Hours { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
