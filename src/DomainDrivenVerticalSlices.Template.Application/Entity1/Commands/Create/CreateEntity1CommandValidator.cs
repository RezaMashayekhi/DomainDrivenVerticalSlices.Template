namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;

using FluentValidation;

public class CreateEntity1CommandValidator : AbstractValidator<CreateEntity1Command>
{
    public CreateEntity1CommandValidator()
    {
        RuleFor(x => x.Property1)
            .NotEmpty();
    }
}
