using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class ApiDocumentConfigurations
{
    public static void RegisterApiDocumentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApiDocument(config =>
        {
            config.Title = "Venice.Orders.Api";
            config.Description = "API para demonstração de JWT";

            config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Insira seu token JWT no formato: Bearer {token}"
            });

            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
    }
    
    public static void UseOpenApiServices(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (!environment.IsDevelopment()) return;
        app.UseOpenApi();
        app.UseSwaggerUi();
    }
}

