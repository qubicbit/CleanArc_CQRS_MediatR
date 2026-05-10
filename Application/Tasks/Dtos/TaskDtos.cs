
namespace Application.Tasks.Dtos
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
    }

    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
