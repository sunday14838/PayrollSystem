namespace PayrollSystem.API.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public decimal BasicSalary { get; set; }

        public int DepartmentId { get; set; }
    }
}
