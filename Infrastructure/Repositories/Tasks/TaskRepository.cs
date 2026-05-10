
using Application.Tasks.Interfaces;

using Domain.Models.Tasks;

using Infrastructure.Database;
using Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tasks;

public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // lägg till special metoder, metoder som IGenericRepo inte har.ex:

    public async Task<List<TaskItem>> GetTasksByUserIdAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

}

