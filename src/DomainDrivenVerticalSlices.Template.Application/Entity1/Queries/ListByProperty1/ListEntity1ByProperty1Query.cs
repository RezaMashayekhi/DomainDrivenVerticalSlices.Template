namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public record ListEntity1ByProperty1Query(string Property1)
    : IRequest<Result<IEnumerable<Entity1Dto>>>;
