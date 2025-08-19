using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Core.Handlers.Commands;

public class OrderCreatedEvent : Event
{
    public required string Id { get; init; }
    public required CustomerOrderCreatedEvent Customer { get; init; }
    public required List<OrderItemOrderCreatedEvent> Items { get; init; }
    public required DateTime Date { get; init; }
    public required StatusOrderCreatedEvent Status { get; init; }
}

public enum StatusOrderCreatedEvent
{
    New = 1,
    
}

public record CustomerOrderCreatedEvent
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string? PhoneNumber { get; init; } 
}

public record OrderItemOrderCreatedEvent
{
    public ProductOrderCreatedEvent Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public record ProductOrderCreatedEvent
{
    public required string Id { get; init; }
    public required string Description { get; init; }
}