namespace DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;
using Microsoft.Extensions.Logging;

public class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // The following HashSet is used to keep track of property names that should be excluded from logging.
    private readonly HashSet<string> _ignoreProps =
    [
        // Items can be added to this list based on command/query properties.
        // Example: nameof(CreateEntity1Command.Property1),

        // Additionally, items can also be excluded manually.
        "Password",
        "Email",
        "CreditCard",
        "Token",
        "ApiKey",
        "Secret",
    ];

    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Request}", typeof(TRequest).Name);

        LogProperties(request);

        var response = await next();

        _logger.LogInformation("Handled {Response}", typeof(TResponse).Name);

        if (response is IResult result)
        {
            if (result.IsSuccess)
            {
                _logger.LogInformation("Operation completed successfully!");

                // Try to log value from Result<T> without using dynamic
                var responseType = typeof(TResponse);
                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var valueProperty = responseType.GetProperty("Value");
                    if (valueProperty != null)
                    {
                        var valueResult = valueProperty.GetValue(response);
                        if (valueResult != null)
                        {
                            LogProperties(valueResult);
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("Operation failed!");
                _logger.LogWarning("Error Type: {ErrorType}, ErrorMessage: {ErrorMessage}", result.CheckedError.ErrorType, result.CheckedError.ErrorMessage);
            }
        }

        return response;
    }

    private void LogProperties(object? obj)
    {
        if (obj == null)
        {
            _logger.LogInformation("Current: (null)");
            return;
        }

        // Handle IEnumerable collections directly
        if (obj is System.Collections.IEnumerable enumerable)
        {
            int count = 0;
            foreach (var e in enumerable)
            {
                count++;
            }

            _logger.LogInformation("Count: {Count}", count);
            return;
        }

        // Log properties of a regular object
        var properties = obj.GetType().GetProperties();

        foreach (var prop in properties)
        {
            // Skip if property is an indexer or requires parameters
            if (prop.GetIndexParameters().Length > 0)
            {
                continue;
            }

            // Skip sensitive/ignored properties.
            if (_ignoreProps.Contains(prop.Name))
            {
                continue;
            }

            var propValue = prop.GetValue(obj);

            if (propValue is DateTime dateTimeValue)
            {
                _logger.LogInformation("{Property}: {@Value}", prop.Name, dateTimeValue.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            else
            {
                _logger.LogInformation("{Property}: {@Value}", prop.Name, propValue);
            }
        }
    }
}
