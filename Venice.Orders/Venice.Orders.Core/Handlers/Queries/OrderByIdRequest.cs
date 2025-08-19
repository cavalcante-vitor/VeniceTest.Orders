using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Core.Models;

namespace Venice.Orders.Core.Handlers.Queries;

public record OrderByIdRequest(string Id) : IQuery<OrderQueryResponse>;