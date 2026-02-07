namespace DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Common.Mediator;
using Microsoft.Extensions.Logging;

/// <summary>
/// Pipeline behavior that catches and logs unhandled exceptions from request handlers.
/// Ensures all exceptions are properly logged with request context before being re-thrown.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response from the handler.</typeparam>
public class UnhandledExceptionBehaviour<TRequest, TResponse>(
    ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(
                ex,
                "Unhandled Exception for Request {RequestName} {@Request}",
                requestName,
                request);

            throw;
        }
    }
}
