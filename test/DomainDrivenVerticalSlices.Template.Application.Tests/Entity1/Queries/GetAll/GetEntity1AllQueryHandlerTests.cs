namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.GetAll;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

public class GetEntity1AllQueryHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetEntity1AllQueryHandler>> _loggerMock;

    private readonly GetEntity1AllQueryHandler _handler;

    public GetEntity1AllQueryHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<GetEntity1AllQueryHandler>>();

        _handler = new GetEntity1AllQueryHandler(
            _entity1RepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new GetEntity1AllQueryHandler(
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
            new GetEntity1AllQueryHandler(
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
            new GetEntity1AllQueryHandler(
                _entity1RepositoryMock.Object,
                _loggerMock.Object,
                null!); // Mapper
        });

        exception.ParamName.Should().Be("mapper");
        exception.Message.Should().Contain("mapper");
    }

    [Fact]
    public async Task Handle_ThereAreSomeEntities_AllEntitiesReturn()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("value1").Value).Value;
        var entity2 = Entity1.Create(ValueObject1.Create("value2").Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));
        var entity2Dto = new Entity1Dto(entity2.Id, new ValueObject1Dto(entity2.ValueObject1.Property1));

        var request = new GetEntity1AllQuery();

        _entity1RepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync([entity1, entity2]);

        var dtos = new List<Entity1Dto> { entity1Dto, entity2Dto };

        _mapperMock.Setup(m => m.Map<IEnumerable<Entity1Dto>>(
            It.Is<IEnumerable<Entity1>>(e => e.Contains(entity1) && e.Contains(entity2))))
            .Returns(dtos);

        _mapperMock.Setup(m => m.Map<Entity1Dto>(It.Is<Entity1>(d => d == entity1)))
            .Returns(entity1Dto);

        _mapperMock.Setup(m => m.Map<Entity1Dto>(It.Is<Entity1>(d => d == entity2)))
            .Returns(entity2Dto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<IEnumerable<Entity1Dto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(2);
        result.Value.Should().BeEquivalentTo(dtos);

        _entity1RepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_NoEntitiesFound_ReturnsEmptyList()
    {
        // Arrange
        var request = new GetEntity1AllQuery();

        _entity1RepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync([]);

        _mapperMock.Setup(m => m.Map<IEnumerable<Entity1Dto>>(It.IsAny<IEnumerable<Entity1>>()))
            .Returns([]);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<IEnumerable<Entity1Dto>>>();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();

        _entity1RepositoryMock.Verify(r => r.GetAllAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnFailureResult()
    {
        // Arrange
        _entity1RepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(new GetEntity1AllQuery(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<IEnumerable<Entity1Dto>>>();
        result.IsSuccess.Should().BeFalse();
        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging("Error retrieving all Entity1.", LogLevel.Error, Times.Once());
    }
}
