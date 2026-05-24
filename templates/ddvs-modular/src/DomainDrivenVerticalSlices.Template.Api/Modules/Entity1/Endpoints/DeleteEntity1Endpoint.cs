namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Endpoints;

using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;
using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.DeleteEntity1;

internal sealed class DeleteEntity1Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/entity1/{id:guid}", HandleAsync)
            .WithTags("Entity1")
            .WithName("DeleteEntity1")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        DeleteEntity1Handler handler,
        CancellationToken cancellationToken)
    {
        Result result = await handler.HandleAsync(new DeleteEntity1Request(id), cancellationToken);

        return result.Match(
            Results.NoContent,
            error => error.ToProblemDetailsResult());
    }
}
