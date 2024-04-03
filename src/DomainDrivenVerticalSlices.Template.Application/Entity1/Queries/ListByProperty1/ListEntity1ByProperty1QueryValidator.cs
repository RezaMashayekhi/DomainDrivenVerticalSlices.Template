namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;

using FluentValidation;

public class ListEntity1ByProperty1QueryValidator : AbstractValidator<ListEntity1ByProperty1Query>
{
    public ListEntity1ByProperty1QueryValidator()
    {
        RuleFor(x => x.Property1)
            .NotEmpty();
    }
}
