using MediatR;

namespace Venice.Orders.Core.Abstractions.Messages;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}