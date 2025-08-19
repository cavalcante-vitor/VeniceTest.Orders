using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Core.Mappers;
using Venice.Orders.Core.Models;
using Venice.Orders.Domain.Services;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Core.Handlers.Commands;

public class AuthCreateCommandHandler(IAuthService authService) : ICommandHandler<AuthCreateRequest, TokenResponse>
{
    public Task<TokenResponse> Handle(AuthCreateRequest request,
        CancellationToken cancellationToken)
    {
        if (!authService.IsValidApiKey(request.Header))
        {
            throw new UnauthorizedCustomException();
        }
        var token =  authService.GenerateToken(cancellationToken);

        return Task.FromResult(AuthMapper.ToResponse(token));
    }
}