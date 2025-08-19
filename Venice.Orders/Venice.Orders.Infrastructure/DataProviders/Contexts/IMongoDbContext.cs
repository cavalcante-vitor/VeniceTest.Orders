using MongoDB.Driver;

namespace Venice.Orders.Infrastructure.DataProviders.Contexts;

public interface IMongoDbContext
{
    IMongoCollection<TEntity> GetCollection<TEntity>();
}