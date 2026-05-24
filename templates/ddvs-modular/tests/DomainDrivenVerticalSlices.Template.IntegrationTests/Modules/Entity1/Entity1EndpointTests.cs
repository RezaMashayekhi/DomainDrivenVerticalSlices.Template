namespace DomainDrivenVerticalSlices.Template.IntegrationTests.Modules.Entity1;

using System.Net;
using System.Net.Http.Json;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Contracts;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.GetEntity1ById;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.ListEntity1;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;

public sealed class Entity1EndpointTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostEntity1_WithValidRequest_CreatesEntity1()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/entity1",
            new CreateEntity1Request("post-value"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        CreateEntity1Response? created = await response.Content.ReadFromJsonAsync<CreateEntity1Response>();

        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
    }

    [Fact]
    public async Task GetEntity1ById_WithExistingEntity1_ReturnsEntity1()
    {
        Guid id = await CreateEntity1Async("get-value");

        GetEntity1ByIdResponse? entity1 = await _client.GetFromJsonAsync<GetEntity1ByIdResponse>(
            $"/api/entity1/{id}");

        Assert.NotNull(entity1);
        Assert.Equal(id, entity1.Entity1.Id);
        Assert.Equal("get-value", entity1.Entity1.ValueObject1.Property1);
    }

    [Fact]
    public async Task ListEntity1_ReturnsEntity1()
    {
        await CreateEntity1Async("list-value-one");
        await CreateEntity1Async("list-value-two");

        ListEntity1Response? response = await _client.GetFromJsonAsync<ListEntity1Response>(
            "/api/entity1");

        Assert.NotNull(response);
        Assert.True(response.TotalCount >= 2);
        Assert.Contains(response.Entity1, entity1 => entity1.ValueObject1.Property1 == "list-value-one");
        Assert.Contains(response.Entity1, entity1 => entity1.ValueObject1.Property1 == "list-value-two");
    }

    [Fact]
    public async Task PutEntity1_WithExistingEntity1_UpdatesEntity1()
    {
        Guid id = await CreateEntity1Async("put-value-one");

        HttpResponseMessage response = await _client.PutAsJsonAsync(
            $"/api/entity1/{id}",
            new UpdateEntity1Request("put-value-two"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        UpdateEntity1Response? updated = await response.Content.ReadFromJsonAsync<UpdateEntity1Response>();

        Assert.NotNull(updated);
        Assert.Equal("put-value-two", updated.Entity1.ValueObject1.Property1);
    }

    [Fact]
    public async Task DeleteEntity1_WithExistingEntity1_RemovesEntity1()
    {
        Guid id = await CreateEntity1Async("delete-value");

        HttpResponseMessage deleteResponse = await _client.DeleteAsync($"/api/entity1/{id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        HttpResponseMessage getResponse = await _client.GetAsync($"/api/entity1/{id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private async Task<Guid> CreateEntity1Async(string property1)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/api/entity1",
            new CreateEntity1Request(property1));

        response.EnsureSuccessStatusCode();

        CreateEntity1Response? created = await response.Content.ReadFromJsonAsync<CreateEntity1Response>();

        Assert.NotNull(created);

        return created.Id;
    }
}
