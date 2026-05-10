using System.Linq.Expressions;

using Application.Common.Interfaces;
using Application.Tasks.Dtos;
using Application.Tasks.Interfaces;
using Application.Tasks.Queries.GetAllTasks;

using AutoMapper;

using Domain.Models.Common;
using Domain.Models.Tasks;

using LinqKit;

using MediatR;

namespace Application.Tasks.Queries.GetAllTasks;

public class GetAllTasksQueryHandler
    : IRequestHandler<GetAllTasksQuery, OperationResult<List<TaskResponseDto>>>
{
    private readonly ITaskRepository _repo;
    private readonly IUserContextService _userContext;
    private readonly IMapper _mapper;

    public GetAllTasksQueryHandler(
        ITaskRepository repo,
        IUserContextService userContext,
        IMapper mapper)
    {
        _repo = repo;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<TaskResponseDto>>> Handle(
        GetAllTasksQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (userId is null)
            return OperationResult<List<TaskResponseDto>>.Failure("User not authenticated");

        // Basfilter: endast tasks för inloggad user
        // Hämta bara tasks där TaskItem.UserId matchar den inloggade användaren.
        Expression<Func<TaskItem, bool>> filter = t => t.UserId == Guid.Parse(userId);

        // Lägg till search-filter om det finns
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            filter = filter.And(t => t.Title.Contains(request.Search));
        }

        var tasks = await _repo.FindAsync(filter);

        var dto = _mapper.Map<List<TaskResponseDto>>(tasks);

        return OperationResult<List<TaskResponseDto>>.Success(dto);
    }
}
