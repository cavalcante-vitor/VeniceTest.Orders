using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Venice.Orders.Domain.Enums;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class ValidationCustomInputDataException(IReadOnlyDictionary<string, string> errorsDictionary) : ApplicationCustomException(
    TitleException, TypeException,
    CustomHttpStatusCode.BadRequest,
    JsonSerializer.Serialize(errorsDictionary))
{
    private const string TitleException = "Invalid input data.";
    private const string TypeException = "Invalid-Input-Data-Exception";
}