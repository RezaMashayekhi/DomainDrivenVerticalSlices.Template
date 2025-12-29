namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record FindEntity1ByProperty1Query(string Property1)
    : IQuery<Result<Entity1Dto>>;
