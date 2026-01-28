using Application.DTOs.CourseDTO;
using FluentValidation;

namespace Application.Validators.Course;

public class CourseCreateValidator : AbstractValidator<CourseCreateDto>
{
    public CourseCreateValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course Id cannot be empty")
            .MaximumLength(10);
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course Name cannot be empty")
            .MaximumLength(50);
        RuleFor(x => x.Time)
            .NotEmpty().WithMessage("Time cannot be empty")
            .MaximumLength(500);
    }
}