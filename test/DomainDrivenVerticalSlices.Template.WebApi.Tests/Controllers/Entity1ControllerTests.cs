namespace DomainDrivenVerticalSlices.Template.WebApi.Tests.Controllers;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class Entity1ControllerTests
{
    private readonly Entity1Controller _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public Entity1ControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new Entity1Controller(_mediatorMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenMediatorIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new Entity1Controller(null!);
        });

        Assert.Equal("mediator", exception.ParamName);
        Assert.Contains("mediator", exception.Message);
    }

    [Fact]
    public async Task GetById_ReturnInvalidInput_WhenUserIdIsInvalid()
    {
        // Arrange
        var error = Error.Create(ErrorType.InvalidInput, "Id must not be empty.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1ByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.GetById(Guid.Empty);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task GetById_ReturnOk_WhenEntity1IdIsValidAndEntityIsFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new Entity1Dto(id, new ValueObject1Dto("TestEntity1"));

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1ByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Success(entity1));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entity1, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetById_ReturnNotFound_WhenEntity1IdIsValidAndEntityIsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var error = Error.Create(ErrorType.NotFound, "Entity1 not found.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1ByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(error, ((NotFoundObjectResult)result).Value);
    }

    [Fact]
    public async Task GetById_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1ByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenEntitiesAreFound()
    {
        // Arrange
        var entities = new List<Entity1Dto>
        {
            new(Guid.NewGuid(), new ValueObject1Dto("TestEntity1")),
            new(Guid.NewGuid(), new ValueObject1Dto("TestEntity2")),
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1AllQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Success(entities));

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entities, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenNoEntitiesAreFound()
    {
        // Arrange
        var entities = new List<Entity1Dto>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1AllQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Success(entities));

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entities, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAll_ReturnsBadRequest_WhenAnErrorOccurs()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1AllQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Failure(error));

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task List_ReturnsOk_WhenEntitiesAreFound()
    {
        // Arrange
        var entities = new List<Entity1Dto>
        {
            new(Guid.NewGuid(), new ValueObject1Dto("TestEntity1")),
            new(Guid.NewGuid(), new ValueObject1Dto("TestEntity2")),
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Success(entities));

        // Act
        var result = await _controller.List("TestProperty1");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entities, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task List_ReturnsOk_WhenNoEntitiesAreFound()
    {
        // Arrange
        var entities = new List<Entity1Dto>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Success(entities));

        // Act
        var result = await _controller.List("TestProperty1");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entities, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task List_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Failure(error));

        // Act
        var result = await _controller.List("TestProperty1");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task FindByProperty1_ReturnsOk_WhenEntityFound()
    {
        // Arrange
        var entity1 = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Success(entity1));

        // Act
        var result = await _controller.FindByProperty1("TestQuery");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entity1, ((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task FindByProperty1_ReturnsNotFound_WhenNoEntityNotFound()
    {
        // Arrange
        var error = Error.Create(ErrorType.NotFound, "Entity1 not found.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.FindByProperty1("TestQuery");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(error, ((NotFoundObjectResult)result).Value);
    }

    [Fact]
    public async Task FindByProperty1_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.FindByProperty1("TestQuery");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenEntityIsSuccessfullyCreated()
    {
        // Arrange
        var entityDto = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));
        var command = new CreateEntity1Command(entityDto.ValueObject1.Property1);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Success(entityDto));

        // Act
        var result = await _controller.Create(command);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(entityDto, ((CreatedAtActionResult)result).Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenInvalidInput()
    {
        // Arrange
        var error = Error.Create(ErrorType.InvalidInput, "Invalid input.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.Create(new CreateEntity1Command(string.Empty));

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await _controller.Create(new CreateEntity1Command(string.Empty));

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenEntityIsSuccessfullyUpdated()
    {
        // Arrange
        var entityDto = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));
        var command = new UpdateEntity1Command(entityDto);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _controller.Update(entityDto.Id, command);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenInvalidInput()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateEntity1Command(new Entity1Dto(id, new ValueObject1Dto(" ")));
        var error = Error.Create(ErrorType.InvalidInput, "Invalid input.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");
        var entityDto = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));
        var command = new UpdateEntity1Command(entityDto);

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.Update(entityDto.Id, command);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var entityDto = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));
        var command = new UpdateEntity1Command(entityDto);

        // Act
        var result = await _controller.Update(Guid.NewGuid(), command);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID mismatch", ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSuccessfullyDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        var id = Guid.Empty;

        var error = Error.Create(ErrorType.InvalidInput, "Invalid Input.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(error, ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEntityIsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var error = Error.Create(ErrorType.NotFound, "Entity1 not found.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(error, ((NotFoundObjectResult)result).Value);
    }
}
