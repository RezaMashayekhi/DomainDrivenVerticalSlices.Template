namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

#pragma warning disable S2094 // This empty record is used by MediatR to route requests to the appropriate handler based on type
public record GetEntity1AllQuery :
    IRequest<Result<IEnumerable<Entity1Dto>>>;
#pragma warning restore S2094
