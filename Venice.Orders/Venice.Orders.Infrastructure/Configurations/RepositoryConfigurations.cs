using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Infrastructure.DataProviders.Contexts;
using Venice.Orders.Infrastructure.DataProviders.Repositories;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class RepositoryConfigurations
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDbContext, MongoDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }
}