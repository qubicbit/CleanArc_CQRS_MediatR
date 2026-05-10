using Application.Tasks.Commands.CreateTask;
using Application.Tasks.Commands.DeleteTask;
using Application.Tasks.Commands.UpdateTask;
using Application.Tasks.Dtos;
using Application.Tasks.Queries.GetAllTasks;
using Application.Tasks.Queries.GetAllTasksAdmin;
using Application.Tasks.Queries.GetTaskById;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var result = await _mediator.Send(new GetAllTasksQuery(search));

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result);
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id));

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/all")]
    public async Task<IActionResult> GetAllAdmin()
    {
        var result = await _mediator.Send(new GetAllTasksAdminQuery());

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var result = await _mediator.Send(new CreateTaskCommand(dto));

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return StatusCode(201, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
    {
        var result = await _mediator.Send(new UpdateTaskCommand(id, dto));

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteTaskCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result);
    }


}
