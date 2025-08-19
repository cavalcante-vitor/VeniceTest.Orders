using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task CreateAsync(Order order, CancellationToken cancellationToken = default);
}