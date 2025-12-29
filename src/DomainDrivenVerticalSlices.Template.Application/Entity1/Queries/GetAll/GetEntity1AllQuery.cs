namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record GetEntity1AllQuery :
    IQuery<Result<IEnumerable<Entity1Dto>>>;
