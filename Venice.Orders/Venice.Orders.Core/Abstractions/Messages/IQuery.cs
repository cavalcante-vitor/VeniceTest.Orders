using MediatR;

namespace Venice.Orders.Core.Abstractions.Messages;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}