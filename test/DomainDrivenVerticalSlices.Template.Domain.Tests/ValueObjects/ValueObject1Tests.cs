namespace DomainDrivenVerticalSlices.Template.Domain.Tests.ValueObjects;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

public class ValueObject1Tests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Property1IsEmptyOrWhiteSpace_ShouldFail(string property1)
    {
        // Arrange
        var error = Error.Create(ErrorType.InvalidInput, "property1 cannot be empty.");

        // Act
        var result = ValueObject1.Create(property1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(error.ErrorType, result.Error!.ErrorType);
        Assert.Equal(error.ErrorMessage, result.Error!.ErrorMessage);
    }

    [Theory]
    [InlineData("Some text")]
    [InlineData("Some text with numbers 123")]
    public void Create_ValidProperty1_ReturnsSuccess(string property1)
    {
        // Act
        var result = ValueObject1.Create(property1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(property1, result.Value.Property1);
    }

    [Fact]
    public void ValueObjects_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var value1 = ValueObject1.Create("value").Value;
        var value2 = ValueObject1.Create("value").Value;

        // Assert
        Assert.Equal(value1, value2);
    }

    [Fact]
    public void ValueObjects_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var value1 = ValueObject1.Create("value1").Value;
        var value2 = ValueObject1.Create("value2").Value;

        // Assert
        Assert.NotEqual(value1, value2);
    }
}
