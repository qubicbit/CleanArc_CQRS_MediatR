using Application.Tasks.Dtos;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(Guid Id, UpdateTaskDto Dto)
    : IRequest<OperationResult<TaskResponseDto>>;
