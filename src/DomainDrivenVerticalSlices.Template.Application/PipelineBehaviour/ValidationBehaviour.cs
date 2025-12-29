namespace DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;
using FluentValidation;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators ?? throw new ArgumentNullException(nameof(validators));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).Select(e => e.ErrorMessage).ToList();

            if (failures.Count != 0)
            {
                var error = Error.Create(ErrorType.InvalidInput, failures);

                // Check if TResponse is a Result type and create appropriate failure
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var valueType = typeof(TResponse).GetGenericArguments()[0];
                    var failureMethod = typeof(Result<>).MakeGenericType(valueType).GetMethod("Failure", [typeof(Error)]);
                    if (failureMethod != null)
                    {
                        var result = failureMethod.Invoke(null, [error]);
                        if (result != null)
                        {
                            return (TResponse)result;
                        }
                    }
                }
                else if (typeof(TResponse) == typeof(Result))
                {
                    return (TResponse)(object)Result.Failure(error);
                }

                throw new InvalidOperationException($"Cannot create error response for type {typeof(TResponse)}.");
            }
        }

        return await next();
    }
}
