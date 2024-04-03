namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;

using FluentValidation;

public class DeleteEntity1CommandValidator : AbstractValidator<DeleteEntity1Command>
{
    public DeleteEntity1CommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
