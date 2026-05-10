using Application.Tasks.Dtos;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Queries.GetAllTasks;

//public record GetAllTasksQuery() : IRequest<OperationResult<List<TaskResponseDto>>>;

// med filtering på search
public record GetAllTasksQuery(string? Search = null) : IRequest<OperationResult<List<TaskResponseDto>>>;

