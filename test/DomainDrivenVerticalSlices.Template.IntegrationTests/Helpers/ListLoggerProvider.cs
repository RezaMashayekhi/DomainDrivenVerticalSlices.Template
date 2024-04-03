namespace DomainDrivenVerticalSlices.Template.IntegrationTests.Helpers;

using Microsoft.Extensions.Logging;

public class ListLoggerProvider(List<string> logMessages) : ILoggerProvider
{
    private readonly List<string> _logMessages = logMessages;

    public ILogger CreateLogger(string categoryName)
    {
        return new ListLogger(_logMessages);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}
