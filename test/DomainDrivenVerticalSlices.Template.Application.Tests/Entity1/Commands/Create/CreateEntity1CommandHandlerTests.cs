namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Commands.Create;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.Events;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

public class CreateEntity1CommandHandlerTests
{
    private readonly Mock<IEntity1Repository> _entity1RepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateEntity1CommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPublisher> _publisherMock;

    private readonly CreateEntity1CommandHandler _handler;

    public CreateEntity1CommandHandlerTests()
    {
        _entity1RepositoryMock = new Mock<IEntity1Repository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateEntity1CommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _publisherMock = new Mock<IPublisher>();

        _handler = new CreateEntity1CommandHandler(
            _entity1RepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _mapperMock.Object,
            _publisherMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenEntity1RepositoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateEntity1CommandHandler(
                null!, // Entity1Repository
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _mapperMock.Object,
                _publisherMock.Object);
        });

        exception.ParamName.Should().Be("entity1Repository");
        exception.Message.Should().Contain("entity1Repository");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenUnitOfWorkIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateEntity1CommandHandler(
                Mock.Of<IEntity1Repository>(),
                null!, // IUnitOfWork
                _loggerMock.Object,
                _mapperMock.Object,
                _publisherMock.Object);
        });

        exception.ParamName.Should().Be("unitOfWork");
        exception.Message.Should().Contain("unitOfWork");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenCreateLoggerIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateEntity1CommandHandler(
                Mock.Of<IEntity1Repository>(),
                _unitOfWorkMock.Object,
                null!, // logger
                _mapperMock.Object,
                _publisherMock.Object);
        });

        exception.ParamName.Should().Be("logger");
        exception.Message.Should().Contain("logger");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenMapperIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateEntity1CommandHandler(
                Mock.Of<IEntity1Repository>(),
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                null!, // Mapper
                _publisherMock.Object);
        });

        exception.ParamName.Should().Be("mapper");
        exception.Message.Should().Contain("mapper");
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenPublisherIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new CreateEntity1CommandHandler(
                Mock.Of<IEntity1Repository>(),
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _mapperMock.Object,
                null!); // IPublisher
        });

        exception.ParamName.Should().Be("publisher");
        exception.Message.Should().Contain("publisher");
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateEntity1AndReturnSuccessResult()
    {
        // Arrange
        var request = new CreateEntity1Command("value1");
        var entity1 = Entity1.Create(ValueObject1.Create("value1").Value).Value;
        var entity1Dto = new Entity1Dto(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));

        _mapperMock.Setup(m => m.Map<Entity1Dto>(It.Is<Entity1>(d => d == entity1)))
            .Returns(entity1Dto);

        _entity1RepositoryMock.Setup(r => r.AddAsync(It.IsAny<Entity1>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(entity1));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<Entity1Dto>>();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(entity1Dto);

        _entity1RepositoryMock.Verify(r => r.AddAsync(It.IsAny<Entity1>(), CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.Is<Entity1CreatedEvent>(e => e.Entity1Id == entity1.Id), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new CreateEntity1Command("value1");

        _unitOfWorkMock.Setup(r => r.SaveChangesAsync(CancellationToken.None))
            .Throws<Exception>();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<Entity1Dto>>();
        result.IsSuccess.Should().BeFalse();

        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging("Error creating Entity1", LogLevel.Error, Times.Once());
        _publisherMock.Verify(p => p.Publish(It.IsAny<Entity1CreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidValueObject1_ShouldReturnFailureResult()
    {
        // Arrange
        var request = new CreateEntity1Command(string.Empty);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<Entity1Dto>>();
        result.IsSuccess.Should().BeFalse();
        _loggerMock.VerifyLogLevelTotalCalls(LogLevel.Error, Times.Once);
        _loggerMock.VerifyLogging("An error occurred: property1 cannot be empty.", LogLevel.Error, Times.Once());
        _publisherMock.Verify(p => p.Publish(It.IsAny<Entity1CreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
