using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class CacheConfigurations
{
    public static void ConfigureCacheServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["CachingConfig:RedisEndpoint"];
            options.InstanceName = "Venice_Orders_";
        });
    }
}