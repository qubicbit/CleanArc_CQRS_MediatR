using Application.Tasks.Dtos;
using Application.Tasks.Interfaces;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Models.Common;
using Domain.Models.Tasks;
using MediatR;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler
    : IRequestHandler<CreateTaskCommand, OperationResult<TaskResponseDto>>
{
    private readonly ITaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContext;

    public CreateTaskCommandHandler(
        ITaskRepository repo,
        IMapper mapper,
        IUserContextService userContext)
    {
        _repo = repo;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<OperationResult<TaskResponseDto>> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        // Hämta UserId från JWT
        var userId = _userContext.UserId;

        if (userId is null)
            return OperationResult<TaskResponseDto>.Failure("User not authenticated");

        //  Map DTO → Domain
        var entity = _mapper.Map<TaskItem>(request.Dto);

        // Id ska inte komma från klienten, sätter vi de själv
        entity.Id = Guid.NewGuid();

        // IsCompleted ska inte komma från klienten, sätter vi de själv
        entity.IsCompleted = false;

        // Konvertera string till Guid (eftersom UserId i Domain är Guid)
        entity.UserId = Guid.Parse(userId);

        // Spara
        await _repo.AddAsync(entity);

        // Map Domain → Response DTO
        var dto = _mapper.Map<TaskResponseDto>(entity);

        return OperationResult<TaskResponseDto>.Success(dto);
    }
}
