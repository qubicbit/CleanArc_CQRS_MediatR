using System;
using System.Collections.Generic;
using System.Text;

using Application.Tasks.Dtos;

using Domain.Models.Common;

using MediatR;

namespace Application.Tasks.Queries.GetTaskById;

public record GetTaskByIdQuery(Guid Id) : IRequest<OperationResult<TaskResponseDto>>;

