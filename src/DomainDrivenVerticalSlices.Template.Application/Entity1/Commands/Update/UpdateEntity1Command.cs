namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record UpdateEntity1Command(Entity1Dto Entity1)
    : ICommand<Result>;
