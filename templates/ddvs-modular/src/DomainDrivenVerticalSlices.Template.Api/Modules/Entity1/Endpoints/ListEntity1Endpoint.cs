namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Endpoints;

using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;
using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.ListEntity1;
using Microsoft.AspNetCore.Mvc;

internal sealed class ListEntity1Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/entity1", HandleAsync)
            .WithTags("Entity1")
            .WithName("ListEntity1")
            .Produces<ListEntity1Response>();
    }

    private static async Task<IResult> HandleAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        ListEntity1Handler handler,
        CancellationToken cancellationToken)
    {
        ListEntity1Request request = new(page.GetValueOrDefault(1), pageSize.GetValueOrDefault(20));

        Result<ListEntity1Response> result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            Results.Ok,
            error => error.ToProblemDetailsResult());
    }
}
