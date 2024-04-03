﻿namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.FindByProperty1;

using System.Linq.Expressions;
using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using FluentAssertions;
using Moq;

public class FindEntity1ByProperty1QueryHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindEntity1ByProperty1QueryHandler _handler;

    public FindEntity1ByProperty1QueryHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new FindEntity1ByProperty1QueryHandler(
            _entity1RepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new FindEntity1ByProperty1QueryHandler(
                null!, // Entity1Repository
                _mapperMock.Object);
        });

        exception.ParamName.Should().Be("entity1Repository");
        exception.Message.Should().Contain("entity1Repository");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenMapperIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new FindEntity1ByProperty1QueryHandler(
                _entity1RepositoryMock.Object,
                null!); // Mapper
        });

        exception.ParamName.Should().Be("mapper");
        exception.Message.Should().Contain("mapper");
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
        _mapperMock.Setup(mapper => mapper.Map<Entity1Dto>(entity1))
            .Returns(entity1Dto);

        var query = new FindEntity1ByProperty1Query(prop1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(entity1Dto);

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
        result.IsSuccess.Should().BeFalse();
        result.CheckedError.ErrorType.Should().Be(ErrorType.NotFound);
        result.CheckedError.ErrorMessage.Should().Be("Entity not found.");

        _entity1RepositoryMock.Verify(r => r.FindAsync(It.IsAny<Expression<Func<Entity1, bool>>>(), CancellationToken.None), Times.Once);
    }
}
