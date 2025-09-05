namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Events;

using DomainDrivenVerticalSlices.Template.Application.Entity1.Events;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;

public class Entity1CreatedEventHandlerTests
{
    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new Entity1CreatedEventHandler(null!);
        });

        Assert.Equal("logger", exception.ParamName);
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public async Task Should_LogInformation_When_Entity1CreatedEventIsHandled()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<Entity1CreatedEventHandler>>();
        var handler = new Entity1CreatedEventHandler(loggerMock.Object);
        var entity1Id = Guid.NewGuid();

        // Act
        await handler.Handle(new Entity1CreatedEvent(entity1Id), CancellationToken.None);

        // Assert
        loggerMock.VerifyLogLevelTotalCalls(LogLevel.Information, Times.Once);
        loggerMock.VerifyLogging($"Entity1CreatedEvent: A new Entity1 was created with ID {entity1Id}.", LogLevel.Information, Times.Once());
    }
}
