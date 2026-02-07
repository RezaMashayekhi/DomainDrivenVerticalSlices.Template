namespace DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;

using System.Reflection;

/// <summary>
/// Extension methods for WebApplication to support Minimal API endpoints.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Automatically discovers and maps all endpoint groups in the executing assembly.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);

        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType) && !t.IsAbstract);

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app.MapGroup(instance));
            }
        }

        return app;
    }

    /// <summary>
    /// Creates a route group for an endpoint group.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <param name="group">The endpoint group.</param>
    /// <returns>The route group builder.</returns>
    /// <remarks>
    /// Uses /api/v2/ prefix to allow both Controllers and Minimal APIs to coexist.
    /// Controllers use /api/{controller}, Minimal APIs use /api/v2/{group}.
    /// </remarks>
    private static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GroupName ?? group.GetType().Name;

        return app
            .MapGroup($"/api/v2/{groupName}")
            .WithGroupName("v2")
            .WithTags(groupName);
    }
}
