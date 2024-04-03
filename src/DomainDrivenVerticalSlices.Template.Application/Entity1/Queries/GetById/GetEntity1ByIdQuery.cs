namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public record GetEntity1ByIdQuery(Guid Id)
    : IRequest<Result<Entity1Dto>>;
