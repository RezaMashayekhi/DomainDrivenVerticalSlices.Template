namespace DomainDrivenVerticalSlices.Template.Infrastructure.Tests.Data.Interceptors;

using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

public class AuditableEntityInterceptorTests
{
    private static readonly DateTimeOffset CreationTime = new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ModificationTime = new(2026, 2, 1, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task SaveChangesAsync_AddedEntity_SetsAllAuditFields()
    {
        var databaseName = Guid.NewGuid().ToString();
        using var context = CreateContext(databaseName, "creator", CreationTime);

        var entity = new TestAuditableEntity { Name = "created" };
        context.Entities.Add(entity);
        await context.SaveChangesAsync();

        Assert.Equal(CreationTime, entity.Created);
        Assert.Equal("creator", entity.CreatedBy);
        Assert.Equal(CreationTime, entity.LastModified);
        Assert.Equal("creator", entity.LastModifiedBy);
    }

    [Fact]
    public async Task SaveChangesAsync_ModifiedEntity_UpdatesModificationFieldsAndKeepsCreation()
    {
        var databaseName = Guid.NewGuid().ToString();
        Guid entityId;

        using (var creationContext = CreateContext(databaseName, "creator", CreationTime))
        {
            var entity = new TestAuditableEntity { Name = "created" };
            creationContext.Entities.Add(entity);
            await creationContext.SaveChangesAsync();
            entityId = entity.Id;
        }

        using var modificationContext = CreateContext(databaseName, "editor", ModificationTime);
        var storedEntity = await modificationContext.Entities.SingleAsync(e => e.Id == entityId);
        storedEntity.Name = "modified";
        await modificationContext.SaveChangesAsync();

        Assert.Equal(CreationTime, storedEntity.Created);
        Assert.Equal("creator", storedEntity.CreatedBy);
        Assert.Equal(ModificationTime, storedEntity.LastModified);
        Assert.Equal("editor", storedEntity.LastModifiedBy);
    }

    private static TestAuditableDbContext CreateContext(string databaseName, string userId, DateTimeOffset utcNow)
    {
        var options = new DbContextOptionsBuilder<TestAuditableDbContext>()
            .UseInMemoryDatabase(databaseName)
            .AddInterceptors(new AuditableEntityInterceptor(new StubUser(userId), new FixedTimeProvider(utcNow)))
            .Options;

        return new TestAuditableDbContext(options);
    }

    private sealed class TestAuditableEntity : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class TestAuditableDbContext(DbContextOptions<TestAuditableDbContext> options) : DbContext(options)
    {
        public DbSet<TestAuditableEntity> Entities => Set<TestAuditableEntity>();
    }

    private sealed class StubUser(string? id) : IUser
    {
        public string? Id => id;
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
