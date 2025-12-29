namespace DomainDrivenVerticalSlices.Template.Application.Tests.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;
using Microsoft.Extensions.Logging;
using Moq;

public class LoggingBehaviourTests
{
    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger<LoggingBehaviour<ICommand<Unit>, Unit>> nullLogger = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new LoggingBehaviour<ICommand<Unit>, Unit>(nullLogger));
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<Unit>, Unit>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<Unit>, Unit>(loggerMock.Object);
        var request = new RequestStub("TestValue1", "TestValue2");

        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);

        string handlingMessage = $"Handling {typeof(ICommand<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(request, next, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(4));

        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyLogging("TestProp1: TestValue1")
            .VerifyLogging("TestProp2: TestValue2");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_IResultWithError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<IResult>, IResult>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<IResult>, IResult>(loggerMock.Object);
        var request = new ResultRequestStub();

        var errorResult = Result.Failure(Error.Create(ErrorType.InvalidInput, "Something went wrong."));

        RequestHandlerDelegate<IResult> next = () => Task.FromResult<IResult>(errorResult);

        string handlingMessage = $"Handling {typeof(ICommand<IResult>).Name}";
        string handledMessage = $"Handled {typeof(IResult).Name}";

        // Act
        await loggingBehaviour.Handle(request, next, CancellationToken.None);

        // Assert - 2 info logs (handling, handled) and 2 warning logs (failed, error details)
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(2));
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Warning, Times.Exactly(2));
        loggerMock
            .VerifyLogging(handlingMessage, LogLevel.Information)
            .VerifyLogging(handledMessage, LogLevel.Information)
            .VerifyLogging("Operation failed!", LogLevel.Warning)
            .VerifyLogging("Error Type: InvalidInput, ErrorMessage: Something went wrong.", LogLevel.Warning);
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_IResultWithSuccess()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<IResult>, IResult>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<IResult>, IResult>(loggerMock.Object);
        var request = new ResultRequestStub();

        var successResult = Result.Success();

        RequestHandlerDelegate<IResult> next = () => Task.FromResult<IResult>(successResult);

        string handlingMessage = $"Handling {typeof(ICommand<IResult>).Name}";
        string handledMessage = $"Handled {typeof(IResult).Name}";

        // Act
        await loggingBehaviour.Handle(request, next, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(3));
        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyLogging("Operation completed successfully!");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_IValueResultWithError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<IResult<ValueTuple>>, IResult<ValueTuple>>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<IResult<ValueTuple>>, IResult<ValueTuple>>(loggerMock.Object);
        var request = new ValueResultRequestStub<ValueTuple>();

        var errorResult = Result<ValueTuple>.Failure(Error.Create(ErrorType.InvalidInput, "Something went wrong."));

        RequestHandlerDelegate<IResult<ValueTuple>> next = () => Task.FromResult<IResult<ValueTuple>>(errorResult);

        string handlingMessage = $"Handling {typeof(ICommand<IResult<ValueTuple>>).Name}";
        string handledMessage = $"Handled {typeof(IResult<ValueTuple>).Name}";

        // Act
        await loggingBehaviour.Handle(request, next, CancellationToken.None);

        // Assert - 2 info logs (handling, handled) and 2 warning logs (failed, error details)
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(2));
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Warning, Times.Exactly(2));
        loggerMock
            .VerifyLogging(handlingMessage, LogLevel.Information)
            .VerifyLogging(handledMessage, LogLevel.Information)
            .VerifyLogging("Operation failed!", LogLevel.Warning)
            .VerifyLogging("Error Type: InvalidInput, ErrorMessage: Something went wrong.", LogLevel.Warning);
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_IValueResultWithSuccess()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<IResult<ResponseStub>>, IResult<ResponseStub>>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<IResult<ResponseStub>>, IResult<ResponseStub>>(loggerMock.Object);
        var request = new ValueResultRequestStub<ResponseStub>();

        var response = new ResponseStub("prop1", "pass");
        IResult<ResponseStub> result = Result<ResponseStub>.Success(response);

        RequestHandlerDelegate<IResult<ResponseStub>> next = () => Task.FromResult(result);

        string handlingMessage = $"Handling {typeof(ICommand<IResult<ResponseStub>>).Name}";
        string handledMessage = $"Handled {typeof(IResult<ResponseStub>).Name}";

        // Act
        await loggingBehaviour.Handle(request, next, CancellationToken.None);

        // Assert - 3 info logs (handling, handled, success)
        // Note: The value properties are not logged when the response is IResult<T> because
        // the logging behavior only logs properties from Result<T>.Value when it's a Result<T>,
        // but the generic type parameter is IResult<T> which doesn't have that property reflected
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(3));
        loggerMock
            .VerifyLogging(handlingMessage, LogLevel.Information)
            .VerifyLogging(handledMessage, LogLevel.Information)
            .VerifyLogging("Operation completed successfully!", LogLevel.Information);
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_NonIResultResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<Unit>, Unit>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<Unit>, Unit>(loggerMock.Object);
        var request = new EmptyRequestStub();

        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);

        string handlingMessage = $"Handling {typeof(ICommand<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(request, next, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(2));
        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyNotLogged("Operation completed successfully!")
            .VerifyNotLogged("Operation failed!");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_WithARequestIncludesSensitiveAndDateTimeProperties()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<Unit>, Unit>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<Unit>, Unit>(loggerMock.Object);
        var time = DateTime.Now;
        var emptyRequest = new WithSensitiveAndDateTimePropertiesRequestStub("TestValue1", time, "password");

        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);

        string handlingMessage = $"Handling {typeof(ICommand<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(emptyRequest, next, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(4));

        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyLogging("TestProp1: TestValue1")
            .VerifyLogging($"TestProp2: {time:yyyy/MM/dd HH:mm:ss}")
            .VerifyNotLogged("password");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_WithEmptyRequest()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<ICommand<Unit>, Unit>>>();
        var loggingBehaviour = new LoggingBehaviour<ICommand<Unit>, Unit>(loggerMock.Object);
        var emptyRequest = new EmptyRequestStub();

        RequestHandlerDelegate<Unit> next = () => Task.FromResult(Unit.Value);

        string handlingMessage = $"Handling {typeof(ICommand<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(emptyRequest, next, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(2));

        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage);
    }
}
