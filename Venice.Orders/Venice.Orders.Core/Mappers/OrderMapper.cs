using Venice.Orders.Core.Handlers.Commands;
using Venice.Orders.Core.Handlers.Queries;
using Venice.Orders.Core.Models;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.ValueObjects;

namespace Venice.Orders.Core.Mappers;

public static class OrderMapper
{
      public static OrderByIdRequest ToByIdQuery(string id) => new(id);

      public static Order ToOrder(this OrderCreateRequest orderCreateRequest)
      {
          var order = Order.Create(
              Customer.Create(
                  orderCreateRequest.Customer.Id,
                  orderCreateRequest.Customer.Name,
                  orderCreateRequest.Customer.Email,
                  orderCreateRequest.Customer.PhoneNumber));
          
          foreach (var itemRequest in orderCreateRequest.Items)
          {
              var product = Product.Create(itemRequest.Product.Id, itemRequest.Product.Description);
              order.AddItem(product, itemRequest.Quantity, itemRequest.Price);
          }
          
          return order;
      }

      public static OrderCreatedEvent ToEvent(this Order order)
      {
          return new OrderCreatedEvent
          {
              Id = order.Id,
              Customer = new CustomerOrderCreatedEvent
              {
                  Id = order.Customer.Id,
                  Name = order.Customer.Name,
                  Email = order.Customer.Email,
                  PhoneNumber = order.Customer.PhoneNumber
              },
              Items = order.Items.Select(item => new OrderItemOrderCreatedEvent
              {
                  Product = new ProductOrderCreatedEvent
                  {
                      Id = item.ProductId,
                      Description = item.ProductName,
                  },
                  Price = item.ProductPrice,
                  Quantity = item.Quantity,

              }).ToList(),
              Date = order.Date,
              Status = (StatusOrderCreatedEvent)order.Status,
          };
      }
    public static OrderQueryResponse ToResponse(this Order entity)
    {
        var orderQueryResponse = new OrderQueryResponse
        {
            Id = entity.Id,
            Customer = new CustomerOrderQueryResponse
            {
                Id = entity.Customer.Id,
                Name = entity.Customer.Name,
                Email = entity.Customer.Email,
                PhoneNumber = entity.Customer.PhoneNumber
            },
            Items = entity.Items.Select(item => new OrderItemOrderQueryResponse
            {
                Product = new ProductOrderQueryResponse
                {
                    Id = item.ProductId,
                    Description = item.ProductName,
                },
                Price = item.ProductPrice,
                Quantity = item.Quantity,
            }).ToList(),
            Date = entity.Date,
            Status = (StatusOrderQueryResponse)
                entity.Status,
        };

        return orderQueryResponse;
    }
}