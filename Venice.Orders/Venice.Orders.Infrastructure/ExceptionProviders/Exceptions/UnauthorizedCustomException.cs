using System.Diagnostics.CodeAnalysis;
using Venice.Orders.Domain.Enums;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

[ExcludeFromCodeCoverage]
public class UnauthorizedCustomException() : ApplicationCustomException(
    TitleException,
    TypeException,
    CustomHttpStatusCode.Unauthorized,
    ExceptionsMessages.Unauthorized.GetDescription())
{
    private const string TitleException = "Invalid authentication credentials.";
    private const string TypeException = "Invalid-Authentication-Exception";
}
