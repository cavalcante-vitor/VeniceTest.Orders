namespace Venice.Orders.Infrastructure.DataProviders.Contexts;

public interface IUnitOfWork
{
    Task ExecuteTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}