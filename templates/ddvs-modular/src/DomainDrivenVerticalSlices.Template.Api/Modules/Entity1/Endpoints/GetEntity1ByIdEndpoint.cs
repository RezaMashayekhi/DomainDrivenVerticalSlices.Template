namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Endpoints;

using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;
using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.GetEntity1ById;

internal sealed class GetEntity1ByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/entity1/{id:guid}", HandleAsync)
            .WithTags("Entity1")
            .WithName("GetEntity1ById")
            .Produces<GetEntity1ByIdResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        GetEntity1ByIdHandler handler,
        CancellationToken cancellationToken)
    {
        Result<GetEntity1ByIdResponse> result = await handler.HandleAsync(id, cancellationToken);

        return result.Match(
            Results.Ok,
            error => error.ToProblemDetailsResult());
    }
}
