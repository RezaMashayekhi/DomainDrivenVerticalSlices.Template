namespace DomainDrivenVerticalSlices.Template.WebApi.Endpoints;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using DomainDrivenVerticalSlices.Template.Common.Errors;
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
        group.MapGet(GetById, "{id:guid}").WithName("GetById");
        group.MapGet(List, "list");
        group.MapGet(Find, "find");
        group.MapPost(Create);
        group.MapPut(Update, "{id:guid}");
        group.MapDelete(Delete, "{id:guid}");
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    internal static async Task<Results<Ok<IEnumerable<Entity1Dto>>, NotFound<Error>, BadRequest<Error>>> GetAll(
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEntity1AllQuery(), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }

    internal static async Task<Results<Ok<Entity1Dto>, NotFound<Error>, BadRequest<Error>>> GetById(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEntity1ByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }

    internal static async Task<Results<Ok<IEnumerable<Entity1Dto>>, NotFound<Error>, BadRequest<Error>>> List(
        string property1,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ListEntity1ByProperty1Query(property1), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }

    internal static async Task<Results<Ok<Entity1Dto>, NotFound<Error>, BadRequest<Error>>> Find(
        string property1,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindEntity1ByProperty1Query(property1), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }

    internal static async Task<Results<CreatedAtRoute<Entity1Dto>, NotFound<Error>, BadRequest<Error>>> Create(
        CreateEntity1Command command,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.CreatedAtRoute(result.Value, "GetById", new { id = result.Value.Id });
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }

    internal static async Task<Results<Ok, NotFound<Error>, BadRequest<Error>>> Update(
        Guid id,
        UpdateEntity1Command command,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        if (id != command.Entity1.Id)
        {
            return TypedResults.BadRequest(Error.Create(Common.Enums.ErrorType.InvalidInput, "ID mismatch"));
        }

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }

    internal static async Task<Results<Ok, NotFound<Error>, BadRequest<Error>>> Delete(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteEntity1Command(id), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        var (statusCode, error) = ApiErrors.Map(result);
        return statusCode == StatusCodes.Status404NotFound
            ? TypedResults.NotFound((Error)error)
            : TypedResults.BadRequest((Error)error);
    }
#pragma warning restore SA1204 // Static elements should appear before instance elements
}
