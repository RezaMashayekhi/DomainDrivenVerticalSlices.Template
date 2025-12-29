namespace DomainDrivenVerticalSlices.Template.Common.Mediator;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Default mediator implementation using Microsoft DI.
/// </summary>
public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <inheritdoc/>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var responseType = typeof(TResponse);
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for {requestType.Name}");

        // Get pipeline behaviors
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
        var behaviors = _serviceProvider.GetServices(behaviorType).Cast<object>().Reverse().ToList();

        // Build the pipeline
        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
        {
            var handleMethod = handlerType.GetMethod("Handle")!;
            return (Task<TResponse>)handleMethod.Invoke(handler, [request, cancellationToken])!;
        };

        foreach (var behavior in behaviors)
        {
            var currentDelegate = handlerDelegate;
            var handleMethod = behaviorType.GetMethod("Handle")!;
            handlerDelegate = () => (Task<TResponse>)handleMethod.Invoke(behavior, [request, currentDelegate, cancellationToken])!;
        }

        return await handlerDelegate();
    }

    /// <inheritdoc/>
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();

        foreach (var handler in handlers)
        {
            await handler.Handle(notification, cancellationToken);
        }
    }
}
