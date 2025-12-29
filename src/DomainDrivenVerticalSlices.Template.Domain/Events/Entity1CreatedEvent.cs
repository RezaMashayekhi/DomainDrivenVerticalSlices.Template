namespace DomainDrivenVerticalSlices.Template.Domain.Events;

using DomainDrivenVerticalSlices.Template.Common.Mediator;

public record Entity1CreatedEvent(Guid Entity1Id) : INotification
{
}
