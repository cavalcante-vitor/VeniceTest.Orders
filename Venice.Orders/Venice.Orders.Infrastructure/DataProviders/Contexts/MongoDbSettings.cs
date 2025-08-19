using System.Diagnostics.CodeAnalysis;

namespace Venice.Orders.Infrastructure.DataProviders.Contexts;

[ExcludeFromCodeCoverage]
public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}