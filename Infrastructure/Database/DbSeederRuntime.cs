using Domain.Models.Tasks;
using Domain.Models.Users;

using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Database;

public static class DbSeederRuntime
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        // Om det redan finns users → gör inget
        if (await context.Users.AnyAsync())
            return;

        // SEED ADMIN
        var admin = CreateUserWithPassword("admin", "admin@example.com", "Admin123!", "Admin");

        // SEED USER
        var user = CreateUserWithPassword("user", "user@example.com", "User123!", "User");

        context.Users.AddRange(admin, user);
        await context.SaveChangesAsync();

        // SEED TASKS FÖR USER
        var task1 = CreateTask("Buy groceries", false, user.Id);
        var task2 = CreateTask("Finish homework", true, user.Id);

        context.Tasks.AddRange(task1, task2);
        await context.SaveChangesAsync();
    }


    private static User CreateUserWithPassword(string username, string email, string password, string role)
    {
        byte[] hash;
        using (var sha = SHA256.Create())
            hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = hash,
            Role = role
        };
    }

    private static TaskItem CreateTask(string title, bool isCompleted, Guid userId)
    {
        return new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            IsCompleted = isCompleted,
            UserId = userId
        };
    }
}
