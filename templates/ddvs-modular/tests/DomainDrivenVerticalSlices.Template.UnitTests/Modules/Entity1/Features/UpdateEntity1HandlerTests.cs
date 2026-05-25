namespace DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1.Features;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;
using DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1;

public sealed class UpdateEntity1HandlerTests
{
    [Fact]
    public async Task HandleAsync_WithExistingEntity1_UpdatesEntity1()
    {
        await using TestDbContextScope scope = await TestDbContextScope.CreateAsync();
        Entity1 entity1 = Entity1.Create(ValueObject1.Create("value-one").Value).Value;
        scope.DbContext.Entities1.Add(entity1);
        await scope.DbContext.SaveChangesAsync();

        UpdateEntity1Handler handler = new(scope.DbContext);

        Result<UpdateEntity1Response> result = await handler.HandleAsync(
            entity1.Id,
            new UpdateEntity1Request("value-two"),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("value-two", result.Value.Entity1.ValueObject1.Property1);
    }

    [Fact]
    public async Task HandleAsync_WithMissingEntity1_ReturnsNotFound()
    {
        await using TestDbContextScope scope = await TestDbContextScope.CreateAsync();
        UpdateEntity1Handler handler = new(scope.DbContext);

        Result<UpdateEntity1Response> result = await handler.HandleAsync(
            Guid.NewGuid(),
            new UpdateEntity1Request("value-two"),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task HandleAsync_WithNullProperty1_ReturnsValidationFailure()
    {
        await using TestDbContextScope scope = await TestDbContextScope.CreateAsync();
        Entity1 entity1 = Entity1.Create(ValueObject1.Create("value-one").Value).Value;
        scope.DbContext.Entities1.Add(entity1);
        await scope.DbContext.SaveChangesAsync();

        UpdateEntity1Handler handler = new(scope.DbContext);

        Result<UpdateEntity1Response> result = await handler.HandleAsync(
            entity1.Id,
            new UpdateEntity1Request(null!),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Contains("Property1", result.Error.ValidationErrors!.Keys);
    }
}
