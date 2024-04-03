namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public record CreateEntity1Command(string Property1)
    : IRequest<Result<Entity1Dto>>;
