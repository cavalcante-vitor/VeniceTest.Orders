using MediatR;

namespace Venice.Orders.Core.Abstractions.Messages;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}