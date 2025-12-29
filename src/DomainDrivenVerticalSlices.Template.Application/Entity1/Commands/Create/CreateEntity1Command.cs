namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record CreateEntity1Command(string Property1)
    : ICommand<Result<Entity1Dto>>;
