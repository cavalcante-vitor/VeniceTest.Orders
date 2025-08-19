using System.Diagnostics.CodeAnalysis;
using Venice.Orders.Domain.Enums;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

[ExcludeFromCodeCoverage]
public class InternalServerErrorCustomException() : ApplicationCustomException(
    TitleException,
    TypeException,
    CustomHttpStatusCode.InternalServerError,
    ExceptionsMessages.InternalServerError.GetDescription())
{
    private const string TitleException = "Unable to complete the request.";
    private const string TypeException = "Internal-Server-Error";
}