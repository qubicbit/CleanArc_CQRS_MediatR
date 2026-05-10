using Domain.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Helpers
{
    public static class ValidationBehaviorSetup
    {
        /// <summary>
        /// Ersätter ASP.NET Core:s standard ModelState-fel med OperationResult.Failure.
        /// Detta gäller ENDAST controllers som använder ModelState (t.ex. AuthController).
        /// MediatR + FluentValidation påverkas inte av detta.
        /// </summary>
        public static IServiceCollection AddCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Detta körs när ModelState är ogiltigt i en controller
                // (t.ex. [ApiController] + [FromBody] DTO utan MediatR)
                options.InvalidModelStateResponseFactory = context =>
                {
                    // Plockar ut alla felmeddelanden från ModelState
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    // Returnerar samma format som resten av API:t
                    // {
                    //   "isSuccess": false,
                    //   "errors": [...],
                    //   "data": null
                    // }
                    var result = OperationResult<string>.Failure(errors);

                    return new BadRequestObjectResult(result);
                };
            });

            return services;
        }
    }
}
