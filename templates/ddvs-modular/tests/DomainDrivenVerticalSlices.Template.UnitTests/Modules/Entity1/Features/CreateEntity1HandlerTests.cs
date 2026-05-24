namespace DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1.Features;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;
using DomainDrivenVerticalSlices.Template.UnitTests.Modules.Entity1;
using Microsoft.EntityFrameworkCore;

public sealed class CreateEntity1HandlerTests
{
    [Fact]
    public async Task HandleAsync_WithUniqueProperty1_CreatesEntity1()
    {
        await using TestDbContextScope scope = await TestDbContextScope.CreateAsync();
        CreateEntity1Handler handler = new(scope.DbContext);

        Result<CreateEntity1Response> result = await handler.HandleAsync(
            new CreateEntity1Request("value-one"),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(1, await scope.DbContext.Entities1.CountAsync());
    }

    [Fact]
    public async Task HandleAsync_WithDuplicateProperty1_ReturnsConflict()
    {
        await using TestDbContextScope scope = await TestDbContextScope.CreateAsync();
        CreateEntity1Handler handler = new(scope.DbContext);

        await handler.HandleAsync(new CreateEntity1Request("value-one"), CancellationToken.None);

        Result<CreateEntity1Response> result = await handler.HandleAsync(
            new CreateEntity1Request("value-one"),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }
}
