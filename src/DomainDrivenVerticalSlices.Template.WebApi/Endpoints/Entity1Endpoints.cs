namespace DomainDrivenVerticalSlices.Template.WebApi.Endpoints;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Minimal API endpoints for Entity1 operations.
/// Alternative to the Entity1Controller using the modern Minimal API approach.
/// </summary>
public class Entity1Endpoints : EndpointGroupBase
{
    /// <inheritdoc/>
    public override string GroupName => "Entity1";

    /// <inheritdoc/>
    public override void Map(RouteGroupBuilder group)
    {
        group.MapGet(GetAll, "all");
        group.MapGet(GetById, "{id:guid}");
        group.MapGet(List, "list");
        group.MapGet(Find, "find");
        group.MapPost(Create);
        group.MapPut(Update, "{id:guid}");
        group.MapDelete(Delete, "{id:guid}");
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    private static async Task<Results<Ok<IEnumerable<Entity1Dto>>, BadRequest<string>>> GetAll(
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEntity1AllQuery(), cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }

    private static async Task<Results<Ok<Entity1Dto>, NotFound, BadRequest<string>>> GetById(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEntity1ByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return result.CheckedError.ErrorType == Common.Enums.ErrorType.NotFound
            ? TypedResults.NotFound()
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }

    private static async Task<Results<Ok<IEnumerable<Entity1Dto>>, BadRequest<string>>> List(
        string property1,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListEntity1ByProperty1Query(property1), cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }

    private static async Task<Results<Ok<Entity1Dto>, NotFound, BadRequest<string>>> Find(
        string property1,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindEntity1ByProperty1Query(property1), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        return result.CheckedError.ErrorType == Common.Enums.ErrorType.NotFound
            ? TypedResults.NotFound()
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }

    private static async Task<Results<Created<Entity1Dto>, BadRequest<string>>> Create(
        CreateEntity1Command command,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/api/v2/Entity1/{result.Value.Id}", result.Value)
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }

    private static async Task<Results<Ok, NotFound, BadRequest<string>>> Update(
        Guid id,
        UpdateEntity1Command command,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        if (id != command.Entity1.Id)
        {
            return TypedResults.BadRequest("ID mismatch");
        }

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return result.CheckedError.ErrorType == Common.Enums.ErrorType.NotFound
            ? TypedResults.NotFound()
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }

    private static async Task<Results<NoContent, NotFound, BadRequest<string>>> Delete(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteEntity1Command(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.NoContent();
        }

        return result.CheckedError.ErrorType == Common.Enums.ErrorType.NotFound
            ? TypedResults.NotFound()
            : TypedResults.BadRequest(result.CheckedError.ErrorMessage);
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
