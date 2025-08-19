using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Domain.Services;
using Venice.Orders.Infrastructure.QueueProviders;
using Venice.Orders.Infrastructure.Services;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class InfrastructureServiceConfigurations
{
    public static void RegisterInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAuthService, AuthService>();
    }
}