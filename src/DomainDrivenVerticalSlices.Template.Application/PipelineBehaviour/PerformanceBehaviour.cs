namespace DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;

using System.Diagnostics;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using Microsoft.Extensions.Logging;

/// <summary>
/// Pipeline behavior that logs warnings for requests that take longer than a threshold to execute.
/// Helps identify performance bottlenecks in the application.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response from the handler.</typeparam>
public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<PerformanceBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Threshold in milliseconds. Requests taking longer than this will be logged as warnings.
    /// </summary>
    private const int WarningThresholdMs = 500;

    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > WarningThresholdMs)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogWarning(
                "Long Running Request: {RequestName} ({ElapsedMilliseconds}ms) {@Request}",
                requestName,
                elapsedMilliseconds,
                request);
        }

        return response;
    }
}
