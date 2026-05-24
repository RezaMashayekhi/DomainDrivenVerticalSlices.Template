namespace DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1.Domain;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;

public sealed class Entity1Tests
{
    [Fact]
    public void Create_WithValidInput_ReturnsEntity1()
    {
        ValueObject1 valueObject1 = ValueObject1.Create("  value-one  ").Value;

        Result<Entity1> result = Entity1.Create(valueObject1);

        Assert.True(result.IsSuccess);
        Assert.Equal("value-one", result.Value.ValueObject1.Property1);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Single(result.Value.DomainEvents);
    }

    [Fact]
    public void Create_WithNullValueObject_ReturnsValidationError()
    {
        Result<Entity1> result = Entity1.Create(null!);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public void Update_WithValidInput_UpdatesEntity1()
    {
        Entity1 entity1 = Entity1.Create(ValueObject1.Create("value-one").Value).Value;
        ValueObject1 updatedValueObject1 = ValueObject1.Create("  value-two  ").Value;

        Result result = entity1.Update(updatedValueObject1);

        Assert.True(result.IsSuccess);
        Assert.Equal("value-two", entity1.ValueObject1.Property1);
    }

    [Fact]
    public void Update_WithNullValueObject_ReturnsValidationError()
    {
        Entity1 entity1 = Entity1.Create(ValueObject1.Create("value-one").Value).Value;

        Result result = entity1.Update(null!);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }
}
