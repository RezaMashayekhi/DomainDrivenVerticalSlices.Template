namespace DomainDrivenVerticalSlices.Template.WebApi.Tests.Endpoints;

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
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.WebApi.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

public class Entity1EndpointsTests
{
    private readonly Mock<ISender> _mediatorMock;

    public Entity1EndpointsTests()
    {
        _mediatorMock = new Mock<ISender>();
    }

    [Fact]
    public async Task GetById_ReturnInvalidInput_WhenEntity1IdIsInvalid()
    {
        // Arrange
        var error = Error.Create(ErrorType.InvalidInput, "Id must not be empty.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1ByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await Entity1Endpoints.GetById(Guid.Empty, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.GetById(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<Entity1Dto>>(result.Result);
        Assert.Equal(entity1, ok.Value);
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
        var result = await Entity1Endpoints.GetById(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var notFound = Assert.IsType<NotFound<Error>>(result.Result);
        Assert.Equal(error, notFound.Value);
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
        var result = await Entity1Endpoints.GetById(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.GetAll(_mediatorMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<IEnumerable<Entity1Dto>>>(result.Result);
        Assert.Equal(entities, ok.Value);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenNoEntitiesAreFound()
    {
        // Arrange
        var entities = new List<Entity1Dto>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1AllQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Success(entities));

        // Act
        var result = await Entity1Endpoints.GetAll(_mediatorMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<IEnumerable<Entity1Dto>>>(result.Result);
        Assert.Equal(entities, ok.Value);
    }

    [Fact]
    public async Task GetAll_ReturnsBadRequest_WhenAnErrorOccurs()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEntity1AllQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Failure(error));

        // Act
        var result = await Entity1Endpoints.GetAll(_mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.List("TestProperty1", _mediatorMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<IEnumerable<Entity1Dto>>>(result.Result);
        Assert.Equal(entities, ok.Value);
    }

    [Fact]
    public async Task List_ReturnsOk_WhenNoEntitiesAreFound()
    {
        // Arrange
        var entities = new List<Entity1Dto>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Success(entities));

        // Act
        var result = await Entity1Endpoints.List("TestProperty1", _mediatorMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<IEnumerable<Entity1Dto>>>(result.Result);
        Assert.Equal(entities, ok.Value);
    }

    [Fact]
    public async Task List_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<Entity1Dto>>.Failure(error));

        // Act
        var result = await Entity1Endpoints.List("TestProperty1", _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
    }

    [Fact]
    public async Task FindByProperty1_ReturnsOk_WhenEntityFound()
    {
        // Arrange
        var entity1 = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Success(entity1));

        // Act
        var result = await Entity1Endpoints.Find("TestQuery", _mediatorMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<Entity1Dto>>(result.Result);
        Assert.Equal(entity1, ok.Value);
    }

    [Fact]
    public async Task FindByProperty1_ReturnsNotFound_WhenEntityIsNotFound()
    {
        // Arrange
        var error = Error.Create(ErrorType.NotFound, "Entity1 not found.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await Entity1Endpoints.Find("TestQuery", _mediatorMock.Object, CancellationToken.None);

        // Assert
        var notFound = Assert.IsType<NotFound<Error>>(result.Result);
        Assert.Equal(error, notFound.Value);
    }

    [Fact]
    public async Task FindByProperty1_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindEntity1ByProperty1Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await Entity1Endpoints.Find("TestQuery", _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.Create(command, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var created = Assert.IsType<CreatedAtRoute<Entity1Dto>>(result.Result);
        Assert.Equal(entityDto, created.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenInvalidInput()
    {
        // Arrange
        var error = Error.Create(ErrorType.InvalidInput, "Invalid input.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await Entity1Endpoints.Create(new CreateEntity1Command(string.Empty), _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenOperationFails()
    {
        // Arrange
        var error = Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Entity1Dto>.Failure(error));

        // Act
        var result = await Entity1Endpoints.Create(new CreateEntity1Command(string.Empty), _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.Update(entityDto.Id, command, _mediatorMock.Object, CancellationToken.None);

        // Assert
        Assert.IsType<Ok>(result.Result);
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
        var result = await Entity1Endpoints.Update(id, command, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.Update(entityDto.Id, command, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var entityDto = new Entity1Dto(Guid.NewGuid(), new ValueObject1Dto("TestEntity1"));
        var command = new UpdateEntity1Command(entityDto);

        // Act
        var result = await Entity1Endpoints.Update(Guid.NewGuid(), command, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(ErrorType.InvalidInput, badRequest.Value!.ErrorType);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEntityNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entityDto = new Entity1Dto(id, new ValueObject1Dto("TestEntity1"));
        var command = new UpdateEntity1Command(entityDto);
        var error = Error.Create(ErrorType.NotFound, "Entity1 not found.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await Entity1Endpoints.Update(id, command, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var notFound = Assert.IsType<NotFound<Error>>(result.Result);
        Assert.Equal(error, notFound.Value);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSuccessfullyDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEntity1Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await Entity1Endpoints.Delete(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        Assert.IsType<Ok>(result.Result);
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
        var result = await Entity1Endpoints.Delete(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.Delete(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequest<Error>>(result.Result);
        Assert.Equal(error, badRequest.Value);
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
        var result = await Entity1Endpoints.Delete(id, _mediatorMock.Object, CancellationToken.None);

        // Assert
        var notFound = Assert.IsType<NotFound<Error>>(result.Result);
        Assert.Equal(error, notFound.Value);
    }
}
