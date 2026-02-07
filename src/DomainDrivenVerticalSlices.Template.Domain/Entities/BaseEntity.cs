namespace DomainDrivenVerticalSlices.Template.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using DomainDrivenVerticalSlices.Template.Common.Mediator;

/// <summary>
/// Base class for all domain entities.
/// Provides identity (Id) and domain event support.
/// </summary>
public abstract class BaseEntity
{
    private readonly List<INotification> _domainEvents = [];

    /// <summary>
    /// Gets or sets the unique identifier for this entity.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Gets the collection of domain events raised by this entity.
    /// Domain events are dispatched after the entity is persisted.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to be dispatched after persistence.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a specific domain event from the collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    public void RemoveDomainEvent(INotification domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all domain events. Called after events have been dispatched.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
