namespace DomainDrivenVerticalSlices.Template.Common.Mediator;

/// <summary>
/// Marker interface for queries with a response.
/// Queries represent intent to read state without side effects.
/// </summary>
/// <typeparam name="TResponse">The type of response.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
