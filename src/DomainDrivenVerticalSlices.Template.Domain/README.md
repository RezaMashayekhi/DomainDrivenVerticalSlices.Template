# Domain Layer

The Domain layer is at the heart of the application, encapsulating the business logic and rules of the system. It is designed following domain-driven design (DDD) principles to ensure that the business domain is accurately represented and that the application's core functionality is aligned with business requirements.

## Key Components

### Base Classes

- **BaseEntity**: Abstract base class providing identity and domain event collection. All entities can raise domain events that are dispatched after persistence.
- **BaseAuditableEntity**: Extends `BaseEntity` with automatic audit tracking — `CreatedAt`, `CreatedBy`, `ModifiedAt`, and `ModifiedBy` fields are populated automatically via EF Core interceptors.

### Entities

Entities are the primary objects within the domain, each with a unique identifier. `Entity1` demonstrates a domain entity inheriting from `BaseAuditableEntity` for full audit support.

### Events

Domain events signify important changes or actions within the domain. For instance, `Entity1CreatedEvent` indicates the creation of `Entity1`. Events are collected on the entity and dispatched after successful persistence.

### Value Objects

Value Objects are objects that do not have a unique identifier and are used to describe aspects of the domain. `ValueObject1` demonstrates how to create immutable value objects with factory methods and validation.

## BaseEntity Pattern

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    private readonly List<INotification> _domainEvents = [];
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
```

## BaseAuditableEntity Pattern

```csharp
public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}
```

## Working with the Domain

When extending the domain:

1. **Entities** should inherit from `BaseEntity` or `BaseAuditableEntity` depending on audit requirements
2. **Domain Events** should be raised via `AddDomainEvent()` to signal important state changes
3. **Value Objects** should use factory methods with `Result<T>` for validation
