namespace DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1.Domain;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;

public sealed class ValueObject1Tests
{
    [Fact]
    public void Create_WithValidInput_ReturnsValueObject()
    {
        Result<ValueObject1> result = ValueObject1.Create("  value-one  ");

        Assert.True(result.IsSuccess);
        Assert.Equal("value-one", result.Value.Property1);
    }

    [Fact]
    public void Create_WithEmptyProperty1_ReturnsValidationError()
    {
        Result<ValueObject1> result = ValueObject1.Create(" ");

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }
}
