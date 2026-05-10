using Application.Common.Interfaces;
using Application.Tasks.Dtos;
using Application.Tasks.Interfaces;

using AutoMapper;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler
    : IRequestHandler<GetTaskByIdQuery, OperationResult<TaskResponseDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IUserContextService _userContext;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(
        ITaskRepository repository,
        IUserContextService userContext,
        IMapper mapper)
    {
        _repository = repository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<TaskResponseDto>> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Hämta userId från JWT
        var userId = _userContext.UserId;
        if (userId is null)
            return OperationResult<TaskResponseDto>.Failure("User not authenticated");

        // 2. Hämta task från databasen
        var task = await _repository.GetByIdAsync(request.Id);

        if (task is null)
            return OperationResult<TaskResponseDto>.Failure("Task not found");

        // 3. Säkerhet: user får bara se sina egna tasks
        if (task.UserId != Guid.Parse(userId))
            return OperationResult<TaskResponseDto>.Failure("Task not found");

        // 4. Mappa till DTO
        var dto = _mapper.Map<TaskResponseDto>(task);

        // 5. Returnera success
        return OperationResult<TaskResponseDto>.Success(dto);
    }
}
