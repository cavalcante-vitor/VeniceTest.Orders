using Venice.Orders.Core.Handlers.Commands;
using Venice.Orders.Core.Models;

namespace Venice.Orders.Core.Mappers;

public static class AuthMapper
{
    public static AuthCreateRequest ToCreate(string? header)
    {
        return new AuthCreateRequest(header ?? string.Empty);
    }

    public static TokenResponse ToResponse(string accessToken)
    {
        return new TokenResponse(accessToken);
    }
}