using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Domain.Services;

public interface IEventBus
{
    Task Publish<T>(string routingKey, T @event) where T : Event;
}