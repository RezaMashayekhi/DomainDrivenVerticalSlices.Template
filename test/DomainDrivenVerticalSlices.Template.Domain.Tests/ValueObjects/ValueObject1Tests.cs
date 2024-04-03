namespace DomainDrivenVerticalSlices.Template.Domain.Tests.ValueObjects;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using FluentAssertions;

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
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(error);
    }

    [Theory]
    [InlineData("Some text")]
    [InlineData("Some text with numbers 123")]
    public void Create_ValidProperty1_ReturnsSuccess(string property1)
    {
        // Act
        var result = ValueObject1.Create(property1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Property1.Should().Be(property1);
    }

    [Fact]
    public void ValueObjects_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var value1 = ValueObject1.Create("value").Value;
        var value2 = ValueObject1.Create("value").Value;

        // Assert
        value1.Should().Be(value2);
    }

    [Fact]
    public void ValueObjects_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var value1 = ValueObject1.Create("value1").Value;
        var value2 = ValueObject1.Create("value2").Value;

        // Assert
        value1.Should().NotBe(value2);
    }
}
