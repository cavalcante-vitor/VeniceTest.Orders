using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Core.Models;

namespace Venice.Orders.Core.Handlers.Commands;

public sealed record OrderCreateRequest : ICommand<OrderQueryResponse>
{
    public required CustomerOrderCreateRequest Customer { get; init; }
    public required List<ItemsOrderCreateRequest> Items { get; init; }
}

public sealed record ItemsOrderCreateRequest
{
    public required ProductOrderCreateRequest Product { get; init; }
    public required int Quantity { get; init; }
    public required decimal Price { get; init; }
}

public sealed record ProductOrderCreateRequest
{
    public required string Id { get; init; }
    public required string Description { get; init; }
}

public sealed record CustomerOrderCreateRequest
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? PhoneNumber { get; init; }
}