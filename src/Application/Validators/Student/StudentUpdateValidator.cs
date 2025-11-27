using FluentValidation;
using Application.DTOs.StudentDTO;

namespace Application.Validators.Student;
public class StudentUpdateValidator : AbstractValidator<StudentUpdateDto>
{
    public StudentUpdateValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Student ID is required.")
            .MaximumLength(10);

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Gender)
            .NotEmpty()
            .Must(g => g == "Male" || g == "Female")
            .WithMessage("Gender must be Male or Female.");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Birth date must be in the past.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .EmailAddress();

        RuleFor(x => x.Phone)
            .MaximumLength(10)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Address)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}