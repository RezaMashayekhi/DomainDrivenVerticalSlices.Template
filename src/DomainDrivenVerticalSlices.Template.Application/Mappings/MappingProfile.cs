namespace DomainDrivenVerticalSlices.Template.Application.Mappings;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Entity1, Entity1Dto>();
        CreateMap<ValueObject1, ValueObject1Dto>();
    }
}
