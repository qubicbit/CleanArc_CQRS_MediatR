using Application.Tasks.Dtos;
using Application.Tasks.Interfaces;

using AutoMapper;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Queries.GetAllTasksAdmin;

public class GetAllTasksAdminQueryHandler
    : IRequestHandler<GetAllTasksAdminQuery, OperationResult<List<TaskResponseDto>>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTasksAdminQueryHandler(
        ITaskRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<TaskResponseDto>>> Handle(
        GetAllTasksAdminQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetAllAsync();

        var dto = _mapper.Map<List<TaskResponseDto>>(tasks);

        return OperationResult<List<TaskResponseDto>>.Success(dto);
    }
}
