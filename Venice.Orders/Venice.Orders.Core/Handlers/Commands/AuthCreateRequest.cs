using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Core.Models;

namespace Venice.Orders.Core.Handlers.Commands;

public sealed record AuthCreateRequest(string Header) : ICommand<TokenResponse>;
