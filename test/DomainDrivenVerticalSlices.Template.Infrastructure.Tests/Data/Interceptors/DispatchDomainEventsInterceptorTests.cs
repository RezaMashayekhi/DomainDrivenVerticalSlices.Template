namespace DomainDrivenVerticalSlices.Template.Infrastructure.Tests.Data.Interceptors;

using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.Events;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data.Interceptors;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class DispatchDomainEventsInterceptorTests
{
    [Fact]
    public async Task SaveChangesAsync_EntityRaisedEvents_AreDispatchedAndCleared()
    {
        using var connection = OpenInMemoryConnection();
        var publisher = new RecordingPublisher();
        using var context = CreateContext(connection, publisher);

        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        context.Entities1.Add(entity);
        await context.SaveChangesAsync();

        var dispatched = Assert.Single(publisher.Published);
        var createdEvent = Assert.IsType<Entity1CreatedEvent>(dispatched);
        Assert.Equal(entity.Id, createdEvent.Entity1Id);
        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public async Task SaveChangesAsync_DispatchesOnlyAfterEntityIsPersisted()
    {
        using var connection = OpenInMemoryConnection();
        var verificationOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var persistedAtDispatch = false;
        var publisher = new RecordingPublisher(() =>
        {
            using var verificationContext = new AppDbContext(verificationOptions);
            persistedAtDispatch = verificationContext.Entities1.AsNoTracking().Any();
            return Task.CompletedTask;
        });

        using var context = CreateContext(connection, publisher);
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        context.Entities1.Add(entity);
        await context.SaveChangesAsync();

        Assert.Single(publisher.Published);
        Assert.True(persistedAtDispatch, "Domain events must be dispatched after the save completes, not before.");
    }

    [Fact]
    public async Task SaveChangesAsync_WhenSaveIsCancelled_DoesNotDispatchAndKeepsEvents()
    {
        using var connection = OpenInMemoryConnection();
        var publisher = new RecordingPublisher();
        using var context = CreateContext(connection, publisher);

        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        context.Entities1.Add(entity);

        using var cancellationSource = new CancellationTokenSource();
        cancellationSource.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => context.SaveChangesAsync(cancellationSource.Token));

        Assert.Empty(publisher.Published);
        Assert.NotEmpty(entity.DomainEvents);
    }

    private static SqliteConnection OpenInMemoryConnection()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }

    private static AppDbContext CreateContext(SqliteConnection connection, IPublisher publisher)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .AddInterceptors(new DispatchDomainEventsInterceptor(publisher))
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private sealed class RecordingPublisher(Func<Task>? onPublish = null) : IPublisher
    {
        public List<INotification> Published { get; } = [];

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            Published.Add(notification);
            return onPublish?.Invoke() ?? Task.CompletedTask;
        }
    }
}
