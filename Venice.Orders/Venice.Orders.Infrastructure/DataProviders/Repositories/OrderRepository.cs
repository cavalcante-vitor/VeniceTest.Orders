using Microsoft.EntityFrameworkCore;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Infrastructure.DataProviders.Contexts;

namespace Venice.Orders.Infrastructure.DataProviders.Repositories;

public sealed class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await context.Orders
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    
    public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }
}
