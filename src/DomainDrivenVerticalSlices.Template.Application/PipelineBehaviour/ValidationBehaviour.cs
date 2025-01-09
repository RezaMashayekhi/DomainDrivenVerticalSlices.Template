namespace DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using FluentValidation;
using MediatR;

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

                if (typeof(IResult).IsAssignableFrom(typeof(TResponse)))
                {
                    var method = typeof(TResponse)
                        .GetMethod("Failure");
                    if (method != null)
                    {
                        var result = method.Invoke(null, new object[] { error });
                        if (result != null)
                        {
                            return (TResponse)result;
                        }
                    }
                }

                throw new InvalidOperationException($"Cannot create error response for type {typeof(TResponse)}.");
            }
        }

        return await next();
    }
}
