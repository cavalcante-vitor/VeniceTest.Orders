using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Domain.Services;

public interface IOrderService
{
    Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}