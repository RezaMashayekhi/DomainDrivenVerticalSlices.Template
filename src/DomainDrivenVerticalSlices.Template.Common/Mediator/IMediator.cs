namespace DomainDrivenVerticalSlices.Template.Common.Mediator;

/// <summary>
/// Defines a mediator to encapsulate request/response and publishing interaction patterns.
/// </summary>
public interface IMediator : ISender, IPublisher
{
}

/// <summary>
/// Send a request through the mediator pipeline to be handled by a single handler.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Asynchronously send a request to a single handler.
    /// </summary>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="request">Request object.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the send operation. The task result contains the handler response.</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Publish a notification to multiple handlers.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Asynchronously send a notification to multiple handlers.
    /// </summary>
    /// <typeparam name="TNotification">Notification type.</typeparam>
    /// <param name="notification">Notification object.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the publish operation.</returns>
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}
