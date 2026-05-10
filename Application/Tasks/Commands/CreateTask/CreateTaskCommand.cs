using Application.Tasks.Dtos;
using Domain.Models.Common;
using MediatR;

namespace Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(CreateTaskDto Dto) : IRequest<OperationResult<TaskResponseDto>>;
