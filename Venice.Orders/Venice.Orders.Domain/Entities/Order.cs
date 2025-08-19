using Venice.Orders.Domain.Enums;
using Venice.Orders.Domain.ValueObjects;

namespace Venice.Orders.Domain.Entities;

public class Order : Entity, IAggregateRoot
{
    public string Id { get; set; }
    public Customer Customer { get; set; }
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }

    public List<OrderItem> Items { get; set; } = [];

    public Order()
    {
    }

    public Order(string id, Customer customer, DateTime date, OrderStatus status)
    {
        Id = id;
        Customer = customer;
        Date = date;
        Status = status;
    }

    public static Order Create(Customer customer)
    {
        return new Order(Guid.NewGuid().ToString(), customer, DateTime.Now, OrderStatus.New);
    }

    public void AddItem(Product product, int quantity, decimal price) =>
        Items.Add(OrderItem.Create(Id, product.Id, product.Description, price, quantity));
}