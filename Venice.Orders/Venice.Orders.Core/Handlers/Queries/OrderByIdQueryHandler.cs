using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Core.Mappers;
using Venice.Orders.Core.Models;
using Venice.Orders.Domain.Services;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Core.Handlers.Queries;

public class OrderByIdQueryHandler(
    IOrderService orderService)
    : IQueryHandler<OrderByIdRequest, OrderQueryResponse>
{
    public async Task<OrderQueryResponse> Handle(OrderByIdRequest query, CancellationToken cancellationToken)
    {
        var order = await orderService.GetByIdAsync(query.Id, cancellationToken) ?? throw new NotFoundCustomException();

        return order.ToResponse();
    }
}