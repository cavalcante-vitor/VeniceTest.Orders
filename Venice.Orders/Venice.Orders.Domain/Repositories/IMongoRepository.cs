using System.Linq.Expressions;

namespace Venice.Orders.Domain.Repositories;

public interface IMongoRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> filterExpression);

    Task<TEntity> GetByIdAsync(string id);
    Task AddAsync(TEntity entity);
}