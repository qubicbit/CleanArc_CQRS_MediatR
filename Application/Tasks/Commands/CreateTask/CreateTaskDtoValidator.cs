using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Dtos;

using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskCommand>
{
    // DTO:n (CreateTaskDto) är bara en property inuti kommandot. Därför måste validatorn vara: : AbstractValidator<CreateTaskCommand>
    // Och reglerna pekar in i DTO:n:
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title cannot exceed 50 characters");
    }
}
