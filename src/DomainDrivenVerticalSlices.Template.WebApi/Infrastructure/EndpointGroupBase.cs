namespace DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;

/// <summary>
/// Base class for Minimal API endpoint groups.
/// Inherit from this class to create organized endpoint groups.
/// </summary>
public abstract class EndpointGroupBase
{
    /// <summary>
    /// Gets the group name for the endpoints.
    /// If null, the class name is used.
    /// </summary>
    public virtual string? GroupName { get; }

    /// <summary>
    /// Maps the endpoints for this group.
    /// </summary>
    /// <param name="groupBuilder">The route group builder.</param>
    public abstract void Map(RouteGroupBuilder groupBuilder);
}
