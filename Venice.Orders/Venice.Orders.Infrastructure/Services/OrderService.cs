using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Domain.Services;
using Venice.Orders.Domain.ValueObjects;
using Venice.Orders.Infrastructure.Extensions;

namespace Venice.Orders.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class OrderService(
    IDistributedCache distributedCache,
    IOrderRepository orderRepository,
    IMongoRepository<OrderItem> orderItemRepository,
    IConfiguration configuration): IOrderService
{
    private enum CacheKeys
    {
        OrderId,
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
       var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(Convert.ToInt64(configuration["CachingConfig:AbsoluteExpirationInSeconds"])));
        
        var order = await distributedCache.GetOrSetAsync(
            id,
            async () =>
            {
                var order = await orderRepository.GetByIdAsync(id, cancellationToken);
                if (order == null) return null;

                var items = await orderItemRepository.FindByAsync(x => x.OrderId == id);
                foreach (var item in items)
                {
                    order.AddItem(Product.Create(item.ProductId, item.ProductName), item.Quantity, item.ProductPrice);
                }
                return order;

            },
        cacheOptions)!;
        return order;
    }
}