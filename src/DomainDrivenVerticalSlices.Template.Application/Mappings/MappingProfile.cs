namespace DomainDrivenVerticalSlices.Template.Application.Mappings;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

public static class MappingProfile
{
    public static Entity1Dto MapToDto(this Entity1 entity)
    {
        return new Entity1Dto(
            entity.Id,
            new ValueObject1Dto(entity.ValueObject1.Property1));
    }

    public static ValueObject1Dto MapToDto(this ValueObject1 valueObject)
    {
        return new ValueObject1Dto(valueObject.Property1);
    }

    public static IEnumerable<Entity1Dto> MapToDto(this IEnumerable<Entity1> entities)
    {
        return entities.Select(MapToDto);
    }
}
