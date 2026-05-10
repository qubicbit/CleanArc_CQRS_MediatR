using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application-assemblyn (används för scanning av handlers, profiler och validators)
        var assembly = typeof(AssemblyReference).Assembly;

        // Registrera MediatR (v12+) och be den scanna hela Application-assemblyn
        // efter alla Commands, Queries och Handlers.
        // Detta ersätter den gamla AddMediatR(assembly)-syntaxen.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // Registrera AutoMapper och scanna Application-assemblyn
        // efter alla klasser som ärver från Profile.
        // Alla mapping-profiler laddas automatiskt.
        // Replace this line:
        // services.AddAutoMapper(assembly);

        // With this line:
        services.AddAutoMapper(cfg => { }, assembly);

        // Registrera FluentValidation och scanna Application-assemblyn
        // efter alla validators (t.ex. CreateTaskDtoValidator).
        services.AddValidatorsFromAssembly(assembly);

        // Registrera ValidationBehavior som pipeline-steg i MediatR.
        // Detta gör att alla Commands/Queries valideras automatiskt
        // innan deras respektive Handlers körs.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Registrera OperationResultBehavior som pipeline-steg u 
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OperationResultBehavior<,>));

        // Pipeline: 1. Validation → 2. OperationResult → 3. ApiSuccess
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ApiSuccessBehavior<,>));



        return services;
    }
}
