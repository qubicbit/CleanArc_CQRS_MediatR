using Domain.Models.Users;

namespace Domain.Models.Tasks;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } // state, ska inte sättas av klienten 

    public User User { get; set; } = null!; // foreign key
    public Guid UserId { get; set; } // navigation property

}
