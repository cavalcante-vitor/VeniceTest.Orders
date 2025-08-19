namespace Venice.Orders.Domain.ValueObjects;

public record Product(string Id, string Description)
{
    public static Product Create(string id, string description) =>
        new (id, description);
    
    public override string ToString() =>
        $"Id: {Id}, Description: {Description}";
}