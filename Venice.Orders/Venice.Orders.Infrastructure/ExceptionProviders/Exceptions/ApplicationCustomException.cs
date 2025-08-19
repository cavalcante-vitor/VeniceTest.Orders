using System.Diagnostics.CodeAnalysis;
using Venice.Orders.Domain.Enums;

namespace Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class ApplicationCustomException(
    string title,
    string type,
    CustomHttpStatusCode statusCode,
    string message) : Exception(message)
{
    public string Title { get; } = title;
    public string Type { get; } = $"/{type}";
    public CustomHttpStatusCode StatusCode { get; } = statusCode;
}