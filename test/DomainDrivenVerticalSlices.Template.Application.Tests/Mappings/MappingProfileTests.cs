namespace DomainDrivenVerticalSlices.Template.Application.Tests.Mappings;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Mappings;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using FluentAssertions;

public class MappingProfileTests
{
    [Fact]
    public void Should_Map_ApplicationUser_To_ApplicationUserDto()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("Property1").Value).Value;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        var mapper = config.CreateMapper();

        // Act
        var dto = mapper.Map<Entity1Dto>(entity1);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(entity1.Id);
        dto.ValueObject1.Should().NotBeNull();
        dto.ValueObject1.Property1.Should().Be(entity1.ValueObject1.Property1);
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

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        var mapper = config.CreateMapper();

        // Act
        var dtoList = mapper.Map<List<Entity1Dto>>(entity1List);

        // Assert
        dtoList.Should().NotBeNull();
        dtoList.Should().HaveCount(entity1List.Count);

        for (int i = 0; i < entity1List.Count; i++)
        {
            var entity1 = entity1List[i];
            var entity1Dto = dtoList[i];

            entity1Dto.Id.Should().Be(entity1.Id);
            entity1Dto.ValueObject1.Should().NotBeNull();
            entity1Dto.ValueObject1.Property1.Should().Be(entity1.ValueObject1.Property1);
        }
    }
}
