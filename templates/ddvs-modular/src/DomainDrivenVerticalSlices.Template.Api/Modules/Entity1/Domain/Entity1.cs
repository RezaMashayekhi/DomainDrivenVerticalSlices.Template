namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.Events;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;

public sealed class Entity1
{
    private readonly List<Entity1CreatedDomainEvent> _domainEvents = [];

    private Entity1()
    {
    }

    private Entity1(ValueObject1 valueObject1)
    {
        Id = Guid.NewGuid();
        ValueObject1 = valueObject1;
        CreatedAtUtc = DateTime.UtcNow;

        _domainEvents.Add(new Entity1CreatedDomainEvent(Id));
    }

    public Guid Id { get; private set; }

    public ValueObject1 ValueObject1 { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<Entity1CreatedDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static Result<Entity1> Create(ValueObject1 valueObject1)
    {
        if (valueObject1 is null)
        {
            return Result<Entity1>.Failure(Entity1Errors.ValueObject1Required());
        }

        return new Entity1(valueObject1);
    }

    public Result Update(ValueObject1 valueObject1)
    {
        if (valueObject1 is null)
        {
            return Result.Failure(Entity1Errors.ValueObject1Required());
        }

        ValueObject1 = valueObject1;

        return Result.Success();
    }
}
