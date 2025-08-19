using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Venice.Orders.Domain;
using Venice.Orders.Domain.Enums;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Infrastructure.ExceptionProviders;

[ExcludeFromCodeCoverage]
public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await HandlerExceptionAsync(httpContext, exception, cancellationToken);
        return true;
    }

    private async Task HandlerExceptionAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var handledException = TransformToHandledException(exception);
        var problemDetails = HandlerProblemDetails(context, handledException);
        HandlerLogger(problemDetails);
        context.Response.ContentType = ApplicationSettings.ErrorResponseContentType;
        context.Response.StatusCode = (int)problemDetails.Status!;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private void HandlerLogger(ProblemDetails problemDetails)
    {
        var statusCode = (CustomHttpStatusCode)problemDetails.Status!;
        var logMessage = JsonSerializer.Serialize(problemDetails);
        if (statusCode == CustomHttpStatusCode.InternalServerError)
        {
            logger.LogError("ProblemDetails: {ProblemDetails}", logMessage);
        }
        else
        {
            logger.LogInformation("ProblemDetails: {ProblemDetails}", logMessage);
        }
    }

    /// <summary>
    /// Generates a <see cref="ProblemDetails"/> object based on the provided application-specific exception and the HTTP context.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/> associated with the HTTP request.</param>
    /// <param name="exception">The <see cref="ApplicationCustomException"/> that contains the details of the handled exception.</param>
    /// <returns>
    /// A <see cref="ProblemDetails"/> instance containing standardized error response information
    /// such as type, title, status, detail, and additional context-specific extensions (e.g., trace and method).
    /// </returns>
    private ProblemDetails HandlerProblemDetails(HttpContext context, ApplicationCustomException exception) =>
        new()
        {
            Type = exception.Type,
            Title = exception.Title,
            Status = (int)exception.StatusCode,
            Detail = exception.Message,
            Instance = context.Request.Path,
            Extensions =
            {
                { "trace", context.TraceIdentifier },
                { "method", context.Request.Method }
            }
        };
    
    private static ApplicationCustomException TransformToHandledException(Exception exception)
    {
        if (exception is OperationCanceledException)
            return new OperationCanceledCustomException();
        
        return exception as ApplicationCustomException 
               ?? new InternalServerErrorCustomException();
    }
}