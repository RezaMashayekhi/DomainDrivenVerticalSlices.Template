namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.ListByProperty1;

using System.Linq.Expressions;
using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

public class ListEntity1ByCriteriaQueryHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<ListEntity1ByProperty1QueryHandler>> _loggerMock;
    private readonly ListEntity1ByProperty1QueryHandler _handler;

    public ListEntity1ByCriteriaQueryHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ListEntity1ByProperty1QueryHandler>>();
        _handler = new ListEntity1ByProperty1QueryHandler(
            _entity1RepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new ListEntity1ByProperty1QueryHandler(
                null!, // Entity1Repository
                _loggerMock.Object,
                _mapperMock.Object);
        });

        exception.ParamName.Should().Be("entity1Repository");
        exception.Message.Should().Contain("entity1Repository");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new ListEntity1ByProperty1QueryHandler(
                _entity1RepositoryMock.Object,
                null!, // Logger
                _mapperMock.Object);
        });
        exception.ParamName.Should().Be("logger");
        exception.Message.Should().Contain("logger");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenMapperIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new ListEntity1ByProperty1QueryHandler(
                _entity1RepositoryMock.Object,
                _loggerMock.Object,
                null!); // Mapper
        });

        exception.ParamName.Should().Be("mapper");
        exception.Message.Should().Contain("mapper");
    }

    [Fact]
    public async Task Handle_EntitiesFound_ReturnsSuccessResultWithDtos()
    {
        // Arrange
        var prop = "Property";
        var entity1 = Entity1.Create(ValueObject1.Create($"{prop}1").Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));

        var entity2 = Entity1.Create(ValueObject1.Create($"{prop}2").Value).Value;
        var entity2Dto = new Entity1Dto(entity2.Id, new ValueObject1Dto(entity2.ValueObject1.Property1));

        var entities = new List<Entity1>() { entity1, entity2 };
        var dtos = new List<Entity1Dto> { entity1Dto, entity2Dto };

        _entity1RepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<Entity1Dto>>(entities))
            .Returns(dtos);

        var query = new ListEntity1ByProperty1Query(prop);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtos);
        _entity1RepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnFailureResult()
    {
        // Arrange
        _entity1RepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));
        var query = new ListEntity1ByProperty1Query("test");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<IEnumerable<Entity1Dto>>>();
        result.IsSuccess.Should().BeFalse();
        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging("Error listing Entity1 by Property1.", LogLevel.Error, Times.Once());
    }
}
