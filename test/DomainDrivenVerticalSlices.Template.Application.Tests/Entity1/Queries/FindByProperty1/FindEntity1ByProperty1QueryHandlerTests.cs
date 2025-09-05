namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.FindByProperty1;

using System.Linq.Expressions;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

public class FindEntity1ByProperty1QueryHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;

    private readonly Mock<ILogger<FindEntity1ByProperty1QueryHandler>> _loggerMock;
    private readonly FindEntity1ByProperty1QueryHandler _handler;

    public FindEntity1ByProperty1QueryHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();

        _loggerMock = new Mock<ILogger<FindEntity1ByProperty1QueryHandler>>();
        _handler = new FindEntity1ByProperty1QueryHandler(
            _entity1RepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new FindEntity1ByProperty1QueryHandler(
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
            new FindEntity1ByProperty1QueryHandler(
                _entity1RepositoryMock.Object,
                null!);
        });
        Assert.Equal("logger", exception.ParamName);
        Assert.Contains("logger", exception.Message);
    }

    [Fact]
    public async Task Handle_EntityFound_ReturnsSuccessResultWithDto()
    {
        // Arrange
        var prop1 = "Property1";
        var entity1 = Entity1.Create(ValueObject1.Create(prop1).Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));

        _entity1RepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity1);

        var query = new FindEntity1ByProperty1Query(prop1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(entity1Dto, result.Value);

        _entity1RepositoryMock.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsFailureResult()
    {
        // Arrange
        var prop1 = "Property1";
        var query = new FindEntity1ByProperty1Query(prop1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.CheckedError.ErrorType);
        Assert.Equal("Entity not found.", result.CheckedError.ErrorMessage);

        _entity1RepositoryMock.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnFailureResult()
    {
        // Arrange
        _entity1RepositoryMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(new FindEntity1ByProperty1Query("test"), CancellationToken.None);

        // Assert
        Assert.IsType<Result<Entity1Dto>>(result);
        Assert.False(result.IsSuccess);
        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging("Error finding Entity1 by Property1.", LogLevel.Error, Times.Once());
    }
}
