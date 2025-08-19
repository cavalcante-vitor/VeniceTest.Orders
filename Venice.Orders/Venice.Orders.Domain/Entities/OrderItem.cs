namespace Venice.Orders.Domain.Entities;

public class OrderItem
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string ProductId { get; set; } 
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    
    public OrderItem() { }
    
    public OrderItem(string orderId, string productId, string productName, decimal productPrice, int quantity) =>
        (OrderId, ProductId, ProductName, ProductPrice, Quantity) = (orderId, productId, productName, productPrice, quantity);
    
    public static OrderItem Create(string orderId, string productId, string productName, decimal productPrice, int quantity) => 
        new (orderId, productId, productName, productPrice, quantity);
}