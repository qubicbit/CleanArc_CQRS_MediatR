using API.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
        {
            // Controllers
            services.AddControllers();

            // Swagger + JWT auth i Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerWithJwtAuth();

            // ModelState → OperationResult
            services.AddCustomValidationResponse();

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // JWT Authentication
            services.AddJwtAuthentication(config);

            // Authorization (kan byggas ut senare)
            services.AddAuthorization();

            return services;
        }
    }
}
