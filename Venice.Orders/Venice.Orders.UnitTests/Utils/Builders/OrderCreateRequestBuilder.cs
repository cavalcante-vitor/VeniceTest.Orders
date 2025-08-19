using Venice.Orders.Core.Handlers.Commands;

namespace Venice.Orders.UnitTests.Utils.Builders;

public class OrderCreateRequestBuilder
{
    private OrderCreateRequest _request;

    private OrderCreateRequestBuilder()
    {
        _request = new OrderCreateRequest
        {
            Customer = new CustomerOrderCreateRequest
            {
                Id = "customer-1",
                Name = "Test Customer",
                Email = "test@example.com",
                PhoneNumber = "11999999999"
            },
            Items =
            [
                new ItemsOrderCreateRequest()
                {
                    Product = new ProductOrderCreateRequest
                    {
                        Id = "product-1",
                        Description = "Test Product"
                    },
                    Quantity = 1,
                    Price = 10.00m
                }
            ]
        };
    }

    public static OrderCreateRequestBuilder Create() => new();

    public OrderCreateRequestBuilder WithCustomer(string id, string name, string email, string phone)
    {
        _request = _request with
        {
            Customer = new CustomerOrderCreateRequest
            {
                Id = id,
                Name = name,
                Email = email,
                PhoneNumber = phone
            }
        };
        
        return this;
    }

    public OrderCreateRequestBuilder WithItems(int count)
    {
        _request = _request with
        {
            Items = Enumerable.Range(1, count)
                .Select(i => new ItemsOrderCreateRequest()
                {
                    Product = new()
                    {
                        Id = $"product-{i}",
                        Description = $"Product {i}"
                    },
                    Quantity = i,
                    Price = i * 10.00m
                })
                .ToList()
        };
        return this;
    }

    public OrderCreateRequestBuilder WithSingleItem(string productId, string description, int quantity, decimal price)
    {
        _request = _request with
        {
            Items =
            [
                new ItemsOrderCreateRequest()
                {
                    Product = new ProductOrderCreateRequest
                    {
                        Id = productId,
                        Description = description
                    },
                    Quantity = quantity,
                    Price = price
                }
            ]
        };
        return this;
    }

    public OrderCreateRequest Build() => _request;
}

