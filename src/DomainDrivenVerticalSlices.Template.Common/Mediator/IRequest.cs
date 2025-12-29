namespace DomainDrivenVerticalSlices.Template.Common.Mediator;

/// <summary>
/// Marker interface for requests with a response.
/// </summary>
/// <typeparam name="TResponse">The type of response.</typeparam>
public interface IRequest<out TResponse>
{
}

/// <summary>
/// Marker interface for requests without a response.
/// </summary>
public interface IRequest : IRequest<Unit>
{
}
