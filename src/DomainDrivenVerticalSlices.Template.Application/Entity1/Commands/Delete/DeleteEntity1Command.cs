namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;

using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

public record DeleteEntity1Command(Guid Id)
    : ICommand<Result>;
