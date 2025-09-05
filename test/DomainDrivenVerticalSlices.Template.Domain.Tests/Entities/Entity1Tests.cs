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
        Assert.IsType<Result<Entity1>>(result);
        Assert.True(result.IsSuccess);

        var entity1 = result.Value;
        Assert.IsType<Entity1>(entity1);
        Assert.NotEqual(Guid.Empty, entity1.Id);
        Assert.Equal(valueObject1, entity1.ValueObject1);
    }
}
