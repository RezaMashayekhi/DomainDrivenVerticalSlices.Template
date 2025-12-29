namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record GetEntity1ByIdQuery(Guid Id)
    : IQuery<Result<Entity1Dto>>;
