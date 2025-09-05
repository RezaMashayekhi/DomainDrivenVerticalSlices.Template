namespace DomainDrivenVerticalSlices.Template.Application.Tests.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

public class LoggingBehaviourTests
{
    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        ILogger<LoggingBehaviour<IRequest<Unit>, Unit>> nullLogger = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new LoggingBehaviour<IRequest<Unit>, Unit>(nullLogger));
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<Unit>, Unit>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<Unit>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<Unit>, Unit>(loggerMock.Object);
        var request = new RequestStub("TestValue1", "TestValue2");

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        string handlingMessage = $"Handling {typeof(IRequest<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(request, handlerMock.Object, CancellationToken.None);

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
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<IResult>, IResult>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<IResult>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<IResult>, IResult>(loggerMock.Object);
        var request = new ResultRequestStub();

        var errorResult = Result.Failure(Error.Create(ErrorType.InvalidInput, "Something went wrong."));

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        string handlingMessage = $"Handling {typeof(IRequest<IResult>).Name}";
        string handledMessage = $"Handled {typeof(IResult).Name}";

        // Act
        await loggingBehaviour.Handle(request, handlerMock.Object, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(4));
        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyLogging("Operation failed!")
            .VerifyLogging("Error Type: InvalidInput, ErrorMessage: Something went wrong.");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_IResultWithSuccess()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<IResult>, IResult>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<IResult>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<IResult>, IResult>(loggerMock.Object);
        var request = new ResultRequestStub();

        var errorResult = Result.Success();

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        string handlingMessage = $"Handling {typeof(IRequest<IResult>).Name}";
        string handledMessage = $"Handled {typeof(IResult).Name}";

        // Act
        await loggingBehaviour.Handle(request, handlerMock.Object, CancellationToken.None);

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
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<IResult<Unit>>, IResult<Unit>>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<IResult<Unit>>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<IResult<Unit>>, IResult<Unit>>(loggerMock.Object);
        var request = new ValueResultRequestStub<Unit>();

        var errorResult = Result<Unit>.Failure(Error.Create(ErrorType.InvalidInput, "Something went wrong."));

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResult);

        string handlingMessage = $"Handling {typeof(IRequest<IResult<Unit>>).Name}";
        string handledMessage = $"Handled {typeof(IResult<Unit>).Name}";

        // Act
        await loggingBehaviour.Handle(request, handlerMock.Object, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(4));
        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyLogging("Operation failed!")
            .VerifyLogging("Error Type: InvalidInput, ErrorMessage: Something went wrong.");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_IValueResultWithSuccess()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<IResult<ResponseStub>>, IResult<ResponseStub>>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<IResult<ResponseStub>>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<IResult<ResponseStub>>, IResult<ResponseStub>>(loggerMock.Object);
        var request = new ValueResultRequestStub<ResponseStub>();

        var response = new ResponseStub("prop1", "pass");
        IResult<ResponseStub> result = Result<ResponseStub>.Success(response);

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        string handlingMessage = $"Handling {typeof(IRequest<IResult<ResponseStub>>).Name}";
        string handledMessage = $"Handled {typeof(IResult<ResponseStub>).Name}";

        // Act
        await loggingBehaviour.Handle(request, handlerMock.Object, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(4));
        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage)
            .VerifyLogging("Prop1: prop1")
            .VerifyLogging("Operation completed successfully!");
    }

    [Fact]
    public async Task Handle_LogsRequestAndResponse_NonIResultResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<Unit>, Unit>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<Unit>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<Unit>, Unit>(loggerMock.Object);
        var request = new EmptyRequestStub();
        var nonIResultResponse = Unit.Value;

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(nonIResultResponse);

        string handlingMessage = $"Handling {typeof(IRequest<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(request, handlerMock.Object, CancellationToken.None);

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
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<Unit>, Unit>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<Unit>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<Unit>, Unit>(loggerMock.Object);
        var time = DateTime.Now;
        var emptyRequest = new WithSensitiveAndDateTimePropertiesRequestStub("TestValue1", time, "password");

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        string handlingMessage = $"Handling {typeof(IRequest<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(emptyRequest, handlerMock.Object, CancellationToken.None);

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
        var loggerMock = new Mock<ILogger<LoggingBehaviour<IRequest<Unit>, Unit>>>();
        var handlerMock = new Mock<RequestHandlerDelegate<Unit>>();
        var loggingBehaviour = new LoggingBehaviour<IRequest<Unit>, Unit>(loggerMock.Object);
        var emptyRequest = new EmptyRequestStub();

        handlerMock
            .Setup(x => x(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        string handlingMessage = $"Handling {typeof(IRequest<Unit>).Name}";
        string handledMessage = $"Handled {typeof(Unit).Name}";

        // Act
        await loggingBehaviour.Handle(emptyRequest, handlerMock.Object, CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Exactly(2));

        loggerMock
            .VerifyLogging(handlingMessage)
            .VerifyLogging(handledMessage);
    }
}
