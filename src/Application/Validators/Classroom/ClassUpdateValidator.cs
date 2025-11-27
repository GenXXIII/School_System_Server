using Application.DTOs.ClassroomDTO;
using FluentValidation;

namespace Application.Validators.Classroom;

public class ClassroomUpdateValidator : AbstractValidator<ClassroomCreateDto>
{
    public ClassroomUpdateValidator()
    {
        RuleFor(x => x.ClassId)
            .NotEmpty().WithMessage("Please specify a class id")
            .MaximumLength(10);
        RuleFor(x => x.Classname)
            .NotEmpty().WithMessage("Please specify a class name")
            .MaximumLength(50);
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Please specify a room number")
            .NotNull();
        RuleFor(x => x.Building)
            .NotEmpty().WithMessage("Please specify a building id")
            .MaximumLength(5);
    }
}