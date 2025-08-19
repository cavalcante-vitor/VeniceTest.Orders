namespace Venice.Orders.Domain.ValueObjects;

public record Customer(string Id, string Name, string Email, string? PhoneNumber)
{
    public static Customer Create(string id, string name, string email,  string? phoneNumber) =>
        new (id, name, email, phoneNumber);
    
    public override string ToString() =>
        $"Id: {Id}, Name: {Name}, E-mail: {Email}, PhoneNumber: {PhoneNumber}";
}