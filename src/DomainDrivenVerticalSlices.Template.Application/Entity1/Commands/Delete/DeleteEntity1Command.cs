namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;

using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public record DeleteEntity1Command(Guid Id)
    : IRequest<Result>;
