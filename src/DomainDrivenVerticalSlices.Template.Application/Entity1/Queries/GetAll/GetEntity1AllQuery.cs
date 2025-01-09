namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public record GetEntity1AllQuery :
    IRequest<Result<IEnumerable<Entity1Dto>>>;
