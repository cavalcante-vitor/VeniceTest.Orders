using System.Linq.Expressions;
using MongoDB.Driver;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Infrastructure.DataProviders.Contexts;

namespace Venice.Orders.Infrastructure.DataProviders.Repositories;

public class MongoRepository<TEntity>(IMongoDbContext context) : IMongoRepository<TEntity>
    where TEntity : class
{
    private readonly IMongoCollection<TEntity> _collection = context.GetCollection<TEntity>();

    public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).ToListAsync();
    }
    
    public async Task<TEntity> GetByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }
}