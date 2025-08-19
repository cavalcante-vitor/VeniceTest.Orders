using Venice.Orders.Api.Configurations;

namespace Venice.Orders.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var environment = builder.Environment;
        builder.Configuration
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.ConfigureApplicationServices(builder.Configuration, environment);

        var app = builder.Build();

        app.UseServices(environment);
        app.MapControllers();
        
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        app.Run();
    }
}