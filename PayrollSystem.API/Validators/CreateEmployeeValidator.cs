using FluentValidation;
using PayrollSystem.API.DTOs.Employees;

namespace PayrollSystem.API.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(30)
                .When(x => !string.IsNullOrWhiteSpace(
                    x.PhoneNumber));

            RuleFor(x => x.JobTitle)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.HireDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.BasicSalary)
                .GreaterThan(0)
                .LessThanOrEqualTo(1_000_000_000);

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0);
        }
    }
}
