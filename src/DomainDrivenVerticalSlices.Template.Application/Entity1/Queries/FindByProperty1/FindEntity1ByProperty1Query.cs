namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public record FindEntity1ByProperty1Query(string Property1)
    : IRequest<Result<Entity1Dto>>;
