namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;

using FluentValidation;

public class FindEntity1ByProperty1QueryValidator : AbstractValidator<FindEntity1ByProperty1Query>
{
    public FindEntity1ByProperty1QueryValidator()
    {
        RuleFor(x => x.Property1)
            .NotEmpty();
    }
}
