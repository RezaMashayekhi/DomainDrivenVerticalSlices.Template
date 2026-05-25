namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;

using FluentValidation;

internal sealed class UpdateEntity1Validator : AbstractValidator<UpdateEntity1Request>
{
    public UpdateEntity1Validator()
    {
        RuleFor(request => request.Property1)
            .NotEmpty()
            .MaximumLength(200);
    }
}
