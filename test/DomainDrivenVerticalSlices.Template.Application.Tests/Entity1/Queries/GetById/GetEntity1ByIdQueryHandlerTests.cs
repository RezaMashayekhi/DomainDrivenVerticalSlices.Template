namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.GetById;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

public class GetEntity1ByIdQueryHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;
    private readonly Mock<ILogger<GetEntity1ByIdQueryHandler>> _loggerMock;

    private readonly GetEntity1ByIdQueryHandler _handler;

    public GetEntity1ByIdQueryHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();
        _loggerMock = new Mock<ILogger<GetEntity1ByIdQueryHandler>>();

        _handler = new GetEntity1ByIdQueryHandler(
            _entity1RepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new GetEntity1ByIdQueryHandler(
                null!, // Entity1Repository
                _loggerMock.Object);
        });

        Assert.Equal("entity1Repository", exception.ParamName);
        Assert.Contains("entity1Repository", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new GetEntity1ByIdQueryHandler(
                _entity1RepositoryMock.Object,
                null!);
        });

        Assert.Equal("logger", exception.ParamName);
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnEntity1()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("value1").Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));
        var request = new GetEntity1ByIdQuery(entity1.Id);

        _entity1RepositoryMock.Setup(r => r.GetByIdAsync(entity1.Id, CancellationToken.None))
            .ReturnsAsync(entity1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Result<Entity1Dto>>(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(entity1Dto, result.Value);

        _entity1RepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_Entity1IsNotFound_ReturnFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new GetEntity1ByIdQuery(id);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Result<Entity1Dto>>(result);
        Assert.False(result.IsSuccess);

        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging($"Entity1 with id {id} not found.", LogLevel.Error, Times.Once());

        _entity1RepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnFailureResult()
    {
        // Arrange
        _entity1RepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));
        var request = new GetEntity1ByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Result<Entity1Dto>>(result);
        Assert.False(result.IsSuccess);
        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging("Error retrieving Entity1 by id.", LogLevel.Error, Times.Once());
    }
}
