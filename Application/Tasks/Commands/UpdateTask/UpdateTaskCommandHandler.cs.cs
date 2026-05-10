using Application.Common.Interfaces;
using Application.Tasks.Dtos;
using Application.Tasks.Interfaces;

using AutoMapper;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler
    : IRequestHandler<UpdateTaskCommand, OperationResult<TaskResponseDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IUserContextService _userContext;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(
        ITaskRepository repository,
        IUserContextService userContext,
        IMapper mapper)
    {
        _repository = repository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<TaskResponseDto>> Handle(
        UpdateTaskCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (userId is null)
            return OperationResult<TaskResponseDto>.Failure("User not authenticated");

        var task = await _repository.GetByIdAsync(request.Id); // finns tasken
        if (task is null)
            return OperationResult<TaskResponseDto>.Failure("Task not found");

        if (task.UserId != Guid.Parse(userId)) // är det rätt user
            return OperationResult<TaskResponseDto>.Failure("Task not found");

        // Uppdatera fält
        task.Title = request.Dto.Title;
        task.IsCompleted = request.Dto.IsCompleted;

        await _repository.UpdateAsync(task);

        var dto = _mapper.Map<TaskResponseDto>(task);
        return OperationResult<TaskResponseDto>.Success(dto);
    }
}
