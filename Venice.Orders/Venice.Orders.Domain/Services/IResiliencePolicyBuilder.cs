namespace Venice.Orders.Domain.Services;

public interface IResiliencePolicyBuilder
{
    Task<T?> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken) where T : class?;
}