namespace DomainDrivenVerticalSlices.Template.Domain.Tests.Entities;

using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

public class Entity1Tests
{
    [Fact]
    public void Create_WithValidInput_ShouldReturnSuccessResultContainingApplicationUserObjectWithCorrectProperties()
    {
        // Arrange
        var property1 = "property1";
        var valueObject1 = ValueObject1.Create(property1).Value;

        // Act
        var result = Entity1.Create(valueObject1);

        // Assert
        result.Should().BeOfType<Result<Entity1>>();
        result.IsSuccess.Should().BeTrue();

        var entity1 = result.Value;
        entity1.Should().BeOfType<Entity1>();
        entity1.Id.Should().NotBeEmpty();
        entity1.ValueObject1.Should().Be(valueObject1);
    }
}
