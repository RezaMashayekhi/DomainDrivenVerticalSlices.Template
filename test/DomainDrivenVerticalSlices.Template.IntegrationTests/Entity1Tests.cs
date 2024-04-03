namespace DomainDrivenVerticalSlices.Template.IntegrationTests;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using DomainDrivenVerticalSlices.Template.IntegrationTests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Entity1Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    private readonly List<string> _logs;

    private readonly int httpsPort;

    private readonly CustomWebApplicationFactory _factory;

    public Entity1Tests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
        _logs = factory.LogMessages;
        httpsPort = factory.Configuration.GetValue<int>("HttpsPort");
        CleanUpDatabase();
    }

    [Fact]
    public async Task GetById_ExistingEntity1_ReturnsEntity()
    {
        // Arrange
        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        var createResponse = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);
        var entity1Dto = await createResponse.Content.ReadFromJsonAsync<Entity1Dto>();

        // Act
        var response = await _httpClient.GetAsync($"/api/entity1/{entity1Dto!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseEntity1Dto = await response.Content.ReadFromJsonAsync<Entity1Dto>();

        entity1Dto.Should().BeEquivalentTo(responseEntity1Dto);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(GetEntity1ByIdQuery)}");
        _logs.Should().Contain($"Id: {entity1Dto.Id}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Id: {entity1Dto.Id}");
        _logs.Should().Contain($"ValueObject1: ValueObject1Dto {{ Property1 = {property1} }}");
    }

    [Fact]
    public async Task GetById_NonExistingEntity1_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"/api/entity1/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(GetEntity1ByIdQuery)}");
        _logs.Should().Contain($"Id: {id}");
        _logs.Should().Contain($"Entity1 with id {id} not found.");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {ErrorType.NotFound}, ErrorMessage: Entity1 with id {id} not found.");
    }

    [Fact]
    public async Task GetById_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var response = await _httpClient.GetAsync($"/api/entity1/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(GetEntity1ByIdQuery)}");
        _logs.Should().Contain($"Id: {id}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {ErrorType.InvalidInput}, ErrorMessage: 'Id' must not be empty.");
    }

    [Fact]
    public async Task GetAll_WhenThereAreSeveralEntities_ReturnsAll()
    {
        // Arrange
        var property1 = "Property1";
        var createCommand1 = new CreateEntity1Command(property1);
        var createResponse1 = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand1);
        var entity1Dto1 = await createResponse1.Content.ReadFromJsonAsync<Entity1Dto>();

        var property2 = "Property2";
        var createCommand2 = new CreateEntity1Command(property2);
        var createResponse2 = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand2);
        var entity1Dto2 = await createResponse2.Content.ReadFromJsonAsync<Entity1Dto>();

        // Act
        var response = await _httpClient.GetAsync("/api/Entity1/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseEntity1Dtos = await response.Content.ReadFromJsonAsync<IEnumerable<Entity1Dto>>();
        responseEntity1Dtos.Should().NotBeNullOrEmpty();
        responseEntity1Dtos!.Count().Should().Be(2);
        responseEntity1Dtos.Should().ContainEquivalentOf(entity1Dto1);
        responseEntity1Dtos.Should().ContainEquivalentOf(entity1Dto2);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(GetEntity1AllQuery)}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Count: {responseEntity1Dtos!.Count()}");
    }

    [Fact]
    public async Task GetAll_WhenNoEntities1_ReturnsEmptyList()
    {
        // Arrange & Act
        var response = await _httpClient.GetAsync("/api/Entity1/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseEntity1Dtos = await response.Content.ReadFromJsonAsync<IEnumerable<Entity1Dto>>();
        responseEntity1Dtos.Should().BeEmpty();

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(GetEntity1AllQuery)}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Count: {responseEntity1Dtos!.Count()}");
    }

    [Fact]
    public async Task List_ReturnsAllItems_WhenMultipleItemsMatchCriteria()
    {
        // Arrange
        var matchingProperty = "Matched";
        var property1 = $"{matchingProperty}1";
        var createCommand1 = new CreateEntity1Command(property1);
        var createResponse1 = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand1);
        var entity1Dto1 = await createResponse1.Content.ReadFromJsonAsync<Entity1Dto>();

        var property2 = $"{matchingProperty}2";
        var createCommand2 = new CreateEntity1Command(property2);
        var createResponse2 = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand2);
        var entity1Dto2 = await createResponse2.Content.ReadFromJsonAsync<Entity1Dto>();

        var property3 = "NotMatching";
        var createCommand3 = new CreateEntity1Command(property3);
        await _httpClient.PostAsJsonAsync("/api/entity1", createCommand3);

        // Act
        var response = await _httpClient.GetAsync($"/api/Entity1/list?property1={matchingProperty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseEntity1Dtos = await response.Content.ReadFromJsonAsync<IEnumerable<Entity1Dto>>();
        responseEntity1Dtos.Should().NotBeNullOrEmpty();
        responseEntity1Dtos!.Count().Should().Be(2);
        responseEntity1Dtos.Should().ContainEquivalentOf(entity1Dto1);
        responseEntity1Dtos.Should().ContainEquivalentOf(entity1Dto2);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(ListEntity1ByProperty1Query)}");
        _logs.Should().Contain($"Property1: {matchingProperty}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Count: {responseEntity1Dtos!.Count()}");
    }

    [Fact]
    public async Task List_ReturnsNoItem_WhenNoItemMatchCriteria()
    {
        // Arrange
        var noMatchingProperty = "NoMatch";

        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);

        // Act
        var response = await _httpClient.GetAsync($"/api/Entity1/list?property1={noMatchingProperty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseEntity1Dtos = await response.Content.ReadFromJsonAsync<IEnumerable<Entity1Dto>>();
        responseEntity1Dtos.Should().BeEmpty();

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(ListEntity1ByProperty1Query)}");
        _logs.Should().Contain($"Property1: {noMatchingProperty}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Count: {responseEntity1Dtos!.Count()}");
    }

    [Fact]
    public async Task Find_ReturnsItem_WhenItemMatchCriteria()
    {
        // Arrange
        var matchingProperty = "Matched";
        var createCommand1 = new CreateEntity1Command(matchingProperty);
        var createResponse1 = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand1);
        var entity1Dto1 = await createResponse1.Content.ReadFromJsonAsync<Entity1Dto>();

        var property2 = "NotMatching";
        var createCommand2 = new CreateEntity1Command(property2);
        await _httpClient.PostAsJsonAsync("/api/entity1", createCommand2);

        // Act
        var response = await _httpClient.GetAsync($"/api/Entity1/find?property1={matchingProperty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseEntity1Dto = await response.Content.ReadFromJsonAsync<Entity1Dto>();
        responseEntity1Dto.Should().BeEquivalentTo(entity1Dto1);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(FindEntity1ByProperty1Query)}");
        _logs.Should().Contain($"Property1: {matchingProperty}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Id: {entity1Dto1!.Id}");
        _logs.Should().Contain($"ValueObject1: ValueObject1Dto {{ Property1 = {matchingProperty} }}");
    }

    [Fact]
    public async Task Find_ReturnsNoItem_WhenItemNotMatchCriteria()
    {
        // Arrange
        var noMatchingProperty = "NoMatch";

        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);

        // Act
        var response = await _httpClient.GetAsync($"/api/Entity1/find?property1={noMatchingProperty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(FindEntity1ByProperty1Query)}");
        _logs.Should().Contain($"Property1: {noMatchingProperty}");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {ErrorType.NotFound}, ErrorMessage: Entity not found.");
    }

    [Fact]
    public async Task Create_ValidInput_ReturnsOkWithEntity1Dto()
    {
        // Arrange
        var property1 = "Property1";
        var command = new CreateEntity1Command(property1);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/entity1", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var entity1Dto = await response.Content.ReadFromJsonAsync<Entity1Dto>();

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.Should().Be($"https://localhost:{httpsPort}/api/Entity1/{entity1Dto!.Id}");

        entity1Dto.Should().NotBeNull();
        entity1Dto.ValueObject1.Property1.Should().Be(property1);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(CreateEntity1Command)}");
        _logs.Should().Contain($"Property1: {property1}");
        _logs.Should().Contain($"Entity1 created with id {entity1Dto.Id}.");
        _logs.Should().Contain($"Entity1CreatedEvent: A new Entity1 was created with ID {entity1Dto.Id}.");
        _logs.Should().Contain("Handled Result`1");
        _logs.Should().Contain("Operation completed successfully!");
        _logs.Should().Contain($"Id: {entity1Dto.Id}");
        _logs.Should().Contain($"ValueObject1: ValueObject1Dto {{ Property1 = {property1} }}");

        // Check if the entity was created in the database
        var getResponse = await _httpClient.GetAsync($"/api/Entity1/{entity1Dto.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponseUserDto = await getResponse.Content.ReadFromJsonAsync<Entity1Dto>();
        entity1Dto.Should().BeEquivalentTo(getResponseUserDto);
    }

    [Fact]
    public async Task Create_InvalidInput_ReturnsBadRequestWithError()
    {
        // Arrange
        var command = new CreateEntity1Command(string.Empty);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/entity1", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ErrorDto>();

        error.Should().NotBeNull();
        error!.ErrorType.Should().Be(ErrorType.InvalidInput);
        error.ErrorMessage.Should().Be("'Property1' must not be empty.");

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(CreateEntity1Command)}");
        _logs.Should().Contain($"Property1: {string.Empty}");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {error.ErrorType}, ErrorMessage: {error.ErrorMessage}");
    }

    [Fact]
    public async Task Update_ValidInput_UpdateTheItem()
    {
        // Arrange
        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        var createResponse = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);
        var entity1Dto = await createResponse.Content.ReadFromJsonAsync<Entity1Dto>();

        var updatedProperty1 = "UpdatedProperty1";
        var updatedEntity1Dto = new Entity1Dto(entity1Dto!.Id, new ValueObject1Dto(updatedProperty1));
        var updateCommand = new UpdateEntity1Command(updatedEntity1Dto);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/entity1/{entity1Dto.Id}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(UpdateEntity1Command)}");
        _logs.Should().Contain($"Id: {entity1Dto.Id}");
        _logs.Should().Contain($"Entity1: Entity1Dto {{ Id = {updatedEntity1Dto.Id}, ValueObject1 = ValueObject1Dto {{ Property1 = {updatedEntity1Dto.ValueObject1.Property1} }} }}");
        _logs.Should().Contain("Handled Result");
        _logs.Should().Contain("Operation completed successfully!");

        // Check if the entity was updated in the database
        var getResponse = await _httpClient.GetAsync($"/api/Entity1/{entity1Dto.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponseUserDto = await getResponse.Content.ReadFromJsonAsync<Entity1Dto>();
        getResponseUserDto!.ValueObject1.Property1.Should().Be(updatedProperty1);
    }

    [Fact]
    public async Task Update_InvalidInput_ReturnsBadRequestWithError()
    {
        // Arrange
        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        var createResponse = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);
        var entity1Dto = await createResponse.Content.ReadFromJsonAsync<Entity1Dto>();

        var updatedEntity1Dto = new Entity1Dto(entity1Dto!.Id, new ValueObject1Dto(string.Empty));
        var updateCommand = new UpdateEntity1Command(updatedEntity1Dto);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/entity1/{entity1Dto.Id}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(UpdateEntity1Command)}");
        _logs.Should().Contain($"Entity1: Entity1Dto {{ Id = {updatedEntity1Dto.Id}, ValueObject1 = ValueObject1Dto {{ Property1 = {updatedEntity1Dto.ValueObject1.Property1} }} }}");
        _logs.Should().Contain("Handled Result");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {ErrorType.InvalidInput}, ErrorMessage: 'Entity1 Value Object1 Property1' must not be empty.");
    }

    [Fact]
    public async Task Update_IdsMisMatched_ReturnsBadRequest()
    {
        // Arrange
        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        var createResponse = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);
        var entity1Dto = await createResponse.Content.ReadFromJsonAsync<Entity1Dto>();

        var updatedEntity1Dto = new Entity1Dto(entity1Dto!.Id, new ValueObject1Dto("UpdatedProperty1"));
        var updateCommand = new UpdateEntity1Command(updatedEntity1Dto);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/entity1/{Guid.NewGuid()}", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("ID mismatch");
    }

    [Fact]
    public async Task Delete_Entity1Exists_DeleteAndReturnsOk()
    {
        // Arrange
        var property1 = "Property1";
        var createCommand = new CreateEntity1Command(property1);
        var createResponse = await _httpClient.PostAsJsonAsync("/api/entity1", createCommand);
        var entity1Dto = await createResponse.Content.ReadFromJsonAsync<Entity1Dto>();

        // Act
        var response = await _httpClient.DeleteAsync($"/api/entity1/{entity1Dto!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(DeleteEntity1Command)}");
        _logs.Should().Contain($"Id: {entity1Dto.Id}");
        _logs.Should().Contain("Handled Result");
        _logs.Should().Contain("Operation completed successfully!");

        // Check if the entity was deleted from the database
        var getResponse = await _httpClient.GetAsync($"/api/Entity1/{entity1Dto.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Entity1NotExists_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _httpClient.DeleteAsync($"/api/entity1/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(DeleteEntity1Command)}");
        _logs.Should().Contain($"Id: {id}");
        _logs.Should().Contain($"Entity1 with id {id} not found.");
        _logs.Should().Contain("Handled Result");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {ErrorType.NotFound}, ErrorMessage: Entity1 with id {id} not found.");
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var response = await _httpClient.DeleteAsync($"/api/entity1/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Log Assert
        _logs.Should().Contain($"Handling {nameof(DeleteEntity1Command)}");
        _logs.Should().Contain($"Id: {id}");
        _logs.Should().Contain("Handled Result");
        _logs.Should().Contain("Operation failed!");
        _logs.Should().Contain($"Error Type: {ErrorType.InvalidInput}, ErrorMessage: 'Id' must not be empty.");
    }

    private void CleanUpDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Entities1.RemoveRange(context.Entities1);
        context.SaveChanges();
    }
}
