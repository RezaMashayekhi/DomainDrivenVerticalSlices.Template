﻿namespace DomainDrivenVerticalSlices.Template.Domain.Entities;

using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

public class Entity1
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Entity1()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Entity1(Guid id, ValueObject1 valueObject1)
    {
        Id = id;
        ValueObject1 = valueObject1;
    }

    public Guid Id { get; private set; }

    public ValueObject1 ValueObject1 { get; private set; }

    public static Result<Entity1> Create(ValueObject1 valueObject1)
    {
        var id = Guid.NewGuid();
        return Result<Entity1>.Success(new Entity1(id, valueObject1));
    }

    public Result<Entity1> Update(ValueObject1 valueObject1)
    {
        ValueObject1 = valueObject1;
        return Result<Entity1>.Success(this);
    }
}
