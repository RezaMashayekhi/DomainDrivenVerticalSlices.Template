namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Events;

using DomainDrivenVerticalSlices.Template.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class Entity1CreatedEventHandler(ILogger<Entity1CreatedEventHandler> logger) : INotificationHandler<Entity1CreatedEvent>
{
    private readonly ILogger<Entity1CreatedEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task Handle(Entity1CreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Entity1CreatedEvent: A new Entity1 was created with ID {Entity1Id}.", notification.Entity1Id);

        return Task.CompletedTask;
    }
}
