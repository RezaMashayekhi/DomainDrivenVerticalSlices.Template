namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;

using FluentValidation;

public class UpdateEntity1CommandValidator : AbstractValidator<UpdateEntity1Command>
{
    public UpdateEntity1CommandValidator()
    {
        RuleFor(x => x.Entity1.ValueObject1.Property1)
            .NotEmpty();
    }
}
