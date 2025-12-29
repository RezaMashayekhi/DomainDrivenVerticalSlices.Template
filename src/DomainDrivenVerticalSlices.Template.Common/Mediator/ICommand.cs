namespace DomainDrivenVerticalSlices.Template.Common.Mediator;

/// <summary>
/// Marker interface for commands with a response.
/// Commands represent intent to change state.
/// </summary>
/// <typeparam name="TResponse">The type of response.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// Marker interface for commands without a response.
/// </summary>
public interface ICommand : ICommand<Unit>
{
}
