using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using Venice.Orders.Core.Abstractions.Messages;
using Venice.Orders.Infrastructure.ExceptionProviders.Exceptions;

namespace Venice.Orders.Core.Behaviors;

[ExcludeFromCodeCoverage]
public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var errorsDictionary = validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().FirstOrDefault(),
                })
            .ToDictionary(x => x.Key, x => x.Values);

        if (errorsDictionary != null && errorsDictionary.Any())
        {
            throw new ValidationCustomInputDataException(errorsDictionary!);
        }

        return await next(cancellationToken);
    }
}