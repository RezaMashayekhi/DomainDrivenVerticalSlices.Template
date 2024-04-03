namespace DomainDrivenVerticalSlices.Template.Domain.Events;

using MediatR;

public record Entity1CreatedEvent(Guid Entity1Id) : INotification
{
}
