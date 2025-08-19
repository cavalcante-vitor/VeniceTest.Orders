using MediatR;

namespace Venice.Orders.Core.Abstractions.Messages;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}