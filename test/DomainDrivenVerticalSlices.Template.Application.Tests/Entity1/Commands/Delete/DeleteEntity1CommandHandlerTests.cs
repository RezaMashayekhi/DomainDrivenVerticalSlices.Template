namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Commands.Delete;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

public class DeleteEntity1CommandHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<DeleteEntity1CommandHandler>> _loggerMock;

    private readonly DeleteEntity1CommandHandler _handler;

    public DeleteEntity1CommandHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteEntity1CommandHandler>>();

        _handler = new DeleteEntity1CommandHandler(
            _entity1RepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new DeleteEntity1CommandHandler(
                null!, // Entity1Repository
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        });

        Assert.Equal("entity1Repository", exception.ParamName);
        Assert.Contains("entity1Repository", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenUnitOfWorkIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new DeleteEntity1CommandHandler(
                _entity1RepositoryMock.Object,
                null!, // UnitOfWork
                _loggerMock.Object);
        });

        Assert.Equal("unitOfWork", exception.ParamName);
        Assert.Contains("unitOfWork", exception.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new DeleteEntity1CommandHandler(
                _entity1RepositoryMock.Object,
                _unitOfWorkMock.Object,
                null!); // Logger
        });

        Assert.Equal("logger", exception.ParamName);
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldDeleteEntity1AndReturnSuccessResult()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("value1").Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));
        var request = new DeleteEntity1Command(entity1Dto.Id);

        _entity1RepositoryMock.Setup(r => r.GetByIdAsync(entity1.Id, CancellationToken.None))
            .ReturnsAsync(entity1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Result>(result);
        Assert.True(result.IsSuccess);

        _entity1RepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Entity1>(), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_EntityIsNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var entity1Dto = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("value1"));
        var request = new DeleteEntity1Command(entity1Dto.Id);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Result>(result);
        Assert.False(result.IsSuccess);

        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging($"Entity1 with id {entity1Dto.Id} not found.", LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnFailureResult()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("value1").Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));
        var request = new DeleteEntity1Command(entity1Dto.Id);

        _entity1RepositoryMock.Setup(r => r.GetByIdAsync(entity1.Id, CancellationToken.None))
            .ReturnsAsync(entity1);

        _unitOfWorkMock.Setup(r => r.SaveChangesAsync(CancellationToken.None))
            .Throws<Exception>();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Result>(result);
        Assert.False(result.IsSuccess);

        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging($"Error deleting Entity1 with id {entity1.Id}.", LogLevel.Error, Times.Once());
    }
}
