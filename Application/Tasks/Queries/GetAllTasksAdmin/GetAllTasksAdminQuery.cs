using Application.Tasks.Dtos;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Queries.GetAllTasksAdmin;

public record GetAllTasksAdminQuery()
    : IRequest<OperationResult<List<TaskResponseDto>>>;
