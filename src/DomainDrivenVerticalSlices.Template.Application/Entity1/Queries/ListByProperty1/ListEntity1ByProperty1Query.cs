namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record ListEntity1ByProperty1Query(string Property1)
    : IQuery<Result<IEnumerable<Entity1Dto>>>;
