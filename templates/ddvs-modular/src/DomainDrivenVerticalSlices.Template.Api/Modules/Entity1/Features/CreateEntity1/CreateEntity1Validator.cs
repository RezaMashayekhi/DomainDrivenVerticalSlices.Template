namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;

using FluentValidation;

internal sealed class CreateEntity1Validator : AbstractValidator<CreateEntity1Request>
{
    public CreateEntity1Validator()
    {
        RuleFor(request => request.Property1)
            .NotEmpty()
            .MaximumLength(200);
    }
}
