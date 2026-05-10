using Application.Common.Interfaces;
using Application.Tasks.Interfaces;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler
    : IRequestHandler<DeleteTaskCommand, OperationResult<bool>>
{
    private readonly ITaskRepository _repository;
    private readonly IUserContextService _userContext;

    public DeleteTaskCommandHandler(
        ITaskRepository repository,
        IUserContextService userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<OperationResult<bool>> Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var isAdmin = _userContext.IsInRole("Admin");

        if (userId is null)
            return OperationResult<bool>.Failure("User not authenticated");

        var task = await _repository.GetByIdAsync(request.Id);
        if (task is null)
            return OperationResult<bool>.Failure("Task not found");

        // User får bara ta bort sina egna tasks
        if (!isAdmin && task.UserId != Guid.Parse(userId))
            return OperationResult<bool>.Failure("Task not found");

        await _repository.DeleteAsync(task);

        return OperationResult<bool>.Success(true);
    }


}
