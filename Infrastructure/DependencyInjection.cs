using Application.Common.Interfaces;
using Application.Tasks.Interfaces;

using Infrastructure.Configuration;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Tasks;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Auth service
        services.AddScoped<IAuthService, AuthService>();

        // Generic repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Feature-specific repositories
        services.AddScoped<ITaskRepository, TaskRepository>();

        // HttpContext accessor + user context
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextService, UserContextService>();

        return services;
    }
}
