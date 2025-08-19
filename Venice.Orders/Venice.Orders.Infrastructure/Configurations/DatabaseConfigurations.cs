using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Infrastructure.DataProviders.Contexts;

namespace Venice.Orders.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public static class DatabaseConfigurations
{
    private const string DefaultConnectionDatabase = "Default";
        
    public static void ConfigureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString(DefaultConnectionDatabase));
        });
        
        MongoDbMappers.RegisterClassMaps();
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDbSettings"));
    }
}