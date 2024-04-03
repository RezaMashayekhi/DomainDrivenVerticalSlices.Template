namespace DomainDrivenVerticalSlices.Template.IntegrationTests.Helpers;

using Microsoft.Extensions.Logging;

public class ListLogger(List<string> logMessages) : ILogger
{
    private readonly List<string> _logMessages = logMessages;

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logMessages.Add(formatter(state, exception));
    }
}
