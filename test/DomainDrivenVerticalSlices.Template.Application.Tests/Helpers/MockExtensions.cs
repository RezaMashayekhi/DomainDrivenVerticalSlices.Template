namespace DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

public static class MockExtensions
{
    public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel = LogLevel.Information, Times? times = null)
    {
        times ??= Times.Once();

        Func<object, Type, bool> state = (v, t) =>
        {
            var actualMessage = v.ToString();
            Debug.WriteLine($"Expected: {expectedMessage}");
            Debug.WriteLine($"Actual: {actualMessage}");
            return string.Compare(actualMessage, expectedMessage, StringComparison.Ordinal) == 0;
        };

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times.Value);

        return logger;
    }

    public static Mock<ILogger<T>> VerifyLogLevelTotalCalls<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel, Times times)
    {
        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);

        return logger;
    }

    public static Mock<ILogger<T>> VerifyLogLevelTotalCalls<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel, Func<Times> times)
    {
        return logger.VerifyLogLevelTotalCalls(expectedLogLevel, times());
    }

    public static Mock<ILogger<T>> VerifyNotLogged<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel = LogLevel.Information)
    {
        return logger.VerifyLogging(expectedMessage, expectedLogLevel, Times.Never());
    }
}
