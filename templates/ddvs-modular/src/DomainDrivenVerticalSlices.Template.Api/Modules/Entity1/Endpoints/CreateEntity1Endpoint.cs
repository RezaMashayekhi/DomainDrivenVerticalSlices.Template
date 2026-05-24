namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Endpoints;

using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;
using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

internal sealed class CreateEntity1Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/entity1", HandleAsync)
            .WithTags("Entity1")
            .WithName("CreateEntity1")
            .Produces<CreateEntity1Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<IResult> HandleAsync(
        CreateEntity1Request request,
        [FromServices] IValidator<CreateEntity1Request> validator,
        [FromServices] CreateEntity1Handler handler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validationResult =
            await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToValidationError().ToProblemDetailsResult();
        }

        Result<CreateEntity1Response> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            success => Results.Created(success.ToLocation(), success),
            error => error.ToProblemDetailsResult());
    }
}
