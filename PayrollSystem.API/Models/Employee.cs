using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollSystem.API.Models
{
    public class Employee
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

        public bool IsActive { get; set; } = true;

        public int DepartmentId { get; set; }

        public int? WorkScheduleId { get; set; }

        public WorkSchedule? WorkSchedule { get; set; }

        // Navigation property
        public Department Department { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
