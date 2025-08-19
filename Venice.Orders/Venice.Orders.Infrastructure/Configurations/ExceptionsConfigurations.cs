using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Infrastructure.ExceptionProviders;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class ExceptionsConfigurations
{
    public static void ConfigureExceptionServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<ExceptionHandlerMiddleware>();
    }

    public static void UseExceptionServices(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
    }
}