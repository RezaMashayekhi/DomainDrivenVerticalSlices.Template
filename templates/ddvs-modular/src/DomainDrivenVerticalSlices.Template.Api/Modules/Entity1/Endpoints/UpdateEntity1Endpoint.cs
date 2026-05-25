namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Endpoints;

using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;
using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

internal sealed class UpdateEntity1Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/entity1/{id:guid}", HandleAsync)
            .WithTags("Entity1")
            .WithName("UpdateEntity1")
            .Produces<UpdateEntity1Response>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        UpdateEntity1Request request,
        [FromServices] IValidator<UpdateEntity1Request> validator,
        [FromServices] UpdateEntity1Handler handler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validationResult =
            await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToValidationError().ToProblemDetailsResult();
        }

        Result<UpdateEntity1Response> result = await handler.HandleAsync(id, request, cancellationToken);

        return result.Match(
            Results.Ok,
            error => error.ToProblemDetailsResult());
    }
}
