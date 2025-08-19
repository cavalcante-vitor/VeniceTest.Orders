using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Venice.Orders.Core.Configurations;
using Venice.Orders.Domain;
using Venice.Orders.Infrastructure.Configurations;

namespace Venice.Orders.Api.Configurations;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.ConfigureExceptionServices();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        services.ConfigureCors();
        services.RegisterApiDocumentServices(configuration);
        services.ConfigureMediatRServices();
        services.ConfigureDatabaseServices(configuration);
        services.RegisterAuthentication(configuration);
        services.RegisterRepositories();
        services.ConfigureCacheServices(configuration);
        services.ConfigureMessageBrokerServices(configuration);
        services.RegisterInfrastructureServices();
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApplicationSettings.CorsPolicyName,
                configurePolicy =>
                {
                    configurePolicy
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public static void UseServices(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseExceptionServices();
        app.UseOpenApiServices(environment);
        app.UseCors(ApplicationSettings.CorsPolicyName);
        app.UseAuthentication();
        app.UseAuthorization();
    }
}