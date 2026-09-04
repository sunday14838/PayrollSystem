namespace PayrollSystem.API.DTOs.Employees
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string EmployeeNumber { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public decimal BasicSalary { get; set; }

        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public int? WorkScheduleId { get; set; }

        public string? WorkScheduleName { get; set; }
    }
}
