using System.Diagnostics.CodeAnalysis;
using Venice.Orders.Domain.Enums;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundCustomException() : ApplicationCustomException(
    TitleException,
    TypeException,
    CustomHttpStatusCode.NotFound,
    ExceptionsMessages.NotFound.GetDescription())
{
    private const string TitleException = "Resource not found.";
    private const string TypeException = "Not-Found-Exception";
}