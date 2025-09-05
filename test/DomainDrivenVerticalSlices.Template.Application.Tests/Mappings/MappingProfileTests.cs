namespace DomainDrivenVerticalSlices.Template.Application.Tests.Mappings;

using DomainDrivenVerticalSlices.Template.Application.Mappings;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

public class MappingProfileTests
{
    [Fact]
    public void Should_Map_Entity1_To_Entity1Dto()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("Property1").Value).Value;

        // Act
        var dto = entity1.MapToDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(entity1.Id, dto.Id);
        Assert.NotNull(dto.ValueObject1);
        Assert.Equal(entity1.ValueObject1.Property1, dto.ValueObject1.Property1);
    }

    [Fact]
    public void Should_Map_ListOfEntity1_To_ListOfEntity1Dto()
    {
        // Arrange
        var entity1List = new List<Entity1>
        {
            Entity1.Create(ValueObject1.Create("Property1").Value).Value,
            Entity1.Create(ValueObject1.Create("Property2").Value).Value,
            Entity1.Create(ValueObject1.Create("Property3").Value).Value,
        };

        // Act
        var dtoList = entity1List.MapToDto();

        // Assert
        Assert.NotNull(dtoList);
        Assert.Equal(entity1List.Count, dtoList.Count());

        for (int i = 0; i < entity1List.Count; i++)
        {
            var entity1 = entity1List[i];
            var entity1Dto = dtoList.ElementAt(i);

            Assert.Equal(entity1.Id, entity1Dto.Id);
            Assert.NotNull(entity1Dto.ValueObject1);
            Assert.Equal(entity1.ValueObject1.Property1, entity1Dto.ValueObject1.Property1);
        }
    }

    [Fact]
    public void Should_Map_ValueObject1_To_ValueObject1Dto()
    {
        // Arrange
        var valueObject1 = ValueObject1.Create("Property1").Value;

        // Act
        var dto = valueObject1.MapToDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(valueObject1.Property1, dto.Property1);
    }
}
