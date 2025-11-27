using Application.DTOs.TeacherDTO;
using FluentValidation;

namespace Application.Validators.Teacher;

public class TeacherCreateValidator : AbstractValidator<TeacherCreateDto>
{
    public TeacherCreateValidator()
    {
        RuleFor(x => x.TeacherId)
            .NotEmpty().WithMessage("Teacher Id cannot be empty")
            .MaximumLength(10);
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name cannot be empty")
            .MaximumLength(50);
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department cannot be empty")
            .MaximumLength(50);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress();
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty")
            .MaximumLength(50);
        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date cannot be empty")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
    }
}