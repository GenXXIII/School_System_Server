using Application.DTOs.DepartmentDTO;
using FluentValidation;

namespace Application.Validators.Department;

public class DepartmentUpdateValidator : AbstractValidator<DepartmentUpdateDto>
{
    public DepartmentUpdateValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Please specify a department ID")
            .MaximumLength(50);

        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Please specify a department name")
            .MaximumLength(50);
            
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("Please specify a room number")
            .NotNull();
            
        RuleFor(x => x.Building)
            .NotEmpty().WithMessage("Please specify a building")
            .MaximumLength(50);
    }
}
