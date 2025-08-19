using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Domain.Services;
using Venice.Orders.Infrastructure.QueueProviders;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class MessageBrokerConfigurations
{
    public static void ConfigureMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus, RabbitMqBusService>();
    }
}