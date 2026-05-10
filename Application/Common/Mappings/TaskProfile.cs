using Application.Tasks.Dtos;

using AutoMapper;

using Domain.Models.Tasks;

namespace Application.Common.Mappings;

public class TaskProfile : Profile
{
    public TaskProfile()
    {

        CreateMap<TaskItem, TaskResponseDto>();

        CreateMap<CreateTaskDto, TaskItem>();


        //CreateMap<UpdateTaskDto, TaskItem>();
    }
}
