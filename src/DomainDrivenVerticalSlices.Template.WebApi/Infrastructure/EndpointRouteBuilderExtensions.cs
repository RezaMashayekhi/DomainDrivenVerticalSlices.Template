namespace DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Extension methods for IEndpointRouteBuilder to simplify Minimal API endpoint registration.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps a GET endpoint with automatic naming based on the handler method.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <param name="handler">The handler delegate.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <returns>The route handler builder.</returns>
    public static RouteHandlerBuilder MapGet(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        return builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name);
    }

    /// <summary>
    /// Maps a POST endpoint with automatic naming based on the handler method.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <param name="handler">The handler delegate.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <returns>The route handler builder.</returns>
    public static RouteHandlerBuilder MapPost(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        return builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name);
    }

    /// <summary>
    /// Maps a PUT endpoint with automatic naming based on the handler method.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <param name="handler">The handler delegate.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <returns>The route handler builder.</returns>
    public static RouteHandlerBuilder MapPut(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern)
    {
        return builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name);
    }

    /// <summary>
    /// Maps a DELETE endpoint with automatic naming based on the handler method.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <param name="handler">The handler delegate.</param>
    /// <param name="pattern">The route pattern.</param>
    /// <returns>The route handler builder.</returns>
    public static RouteHandlerBuilder MapDelete(
        this IEndpointRouteBuilder builder,
        Delegate handler,
        [StringSyntax("Route")] string pattern)
    {
        return builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);
    }
}
