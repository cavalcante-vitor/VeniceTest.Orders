using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Core.Mappers;
using Venice.Orders.Core.Models;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Domain.Services;
using Venice.Orders.Infrastructure.DataProviders.Contexts;

namespace Venice.Orders.Core.Handlers.Commands;

public class OrderCreateCommandHandler(
    IOrderRepository orderRepository,
    IMongoRepository<OrderItem> orderItemRepository,
    IEventBus eventBus,
    IUnitOfWork unitOfWork) : ICommandHandler<OrderCreateRequest, OrderQueryResponse>
{
    private const string RoutingKey = "venice.orders.created";

    public async Task<OrderQueryResponse> Handle(OrderCreateRequest request,
        CancellationToken cancellationToken)
    {
        var order = request.ToOrder();

        await unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await orderRepository.CreateAsync(order, cancellationToken);
                foreach (var item in order.Items)
                {
                    await orderItemRepository.AddAsync(item);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            , cancellationToken);

        //TODO trade-off: Para atualizar atomicamente o banco de dados e enviar mensagem refatorar para utilizar OUTBOX pattern
        await eventBus.Publish(RoutingKey, order.ToEvent());
        return order.ToResponse();
    }
}