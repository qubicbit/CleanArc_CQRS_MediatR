using Application.Common.Interfaces;

using Domain.Models.Tasks;

namespace Application.Tasks.Interfaces;

// Repository = dataåtkomst
// ITaskRepository jobbar direkt mot databasen och hanterar entiteter:
public interface ITaskRepository : IGenericRepository<TaskItem>
{
    // lägg till special metoder, metoder som IGenericRepo inte har.ex:
    Task<List<TaskItem>> GetTasksByUserIdAsync(Guid userId);
}
