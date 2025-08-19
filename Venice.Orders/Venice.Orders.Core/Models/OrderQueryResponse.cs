namespace Venice.Orders.Core.Models;

public record OrderQueryResponse
{
    public required string Id { get; init; }
    public required CustomerOrderQueryResponse Customer { get; init; }
    public required List<OrderItemOrderQueryResponse> Items { get; init; }
    public required DateTime Date { get; init; }
    public required StatusOrderQueryResponse Status { get; init; }

}

public enum StatusOrderQueryResponse
{
    New = 1,
    
}

public record CustomerOrderQueryResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string? PhoneNumber { get; init; } 
}

public record OrderItemOrderQueryResponse
{
    public ProductOrderQueryResponse Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public record ProductOrderQueryResponse
{
    public required string Id { get; init; }
    public required string Description { get; init; }
}