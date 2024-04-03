namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;

using FluentValidation;

public class GetEntity1ByIdQueryValidator : AbstractValidator<GetEntity1ByIdQuery>
{
    public GetEntity1ByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
