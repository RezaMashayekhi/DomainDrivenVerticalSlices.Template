namespace DomainDrivenVerticalSlices.Template.Infrastructure.Tests.Data;

using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class UnitOfWorkTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UnitOfWork _unitOfWork;
    private bool _disposed = false;

    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:;")
            .Options;
        _context = new AppDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _unitOfWork = new UnitOfWork(_context);
    }

    [Fact]
    public async Task Transaction_Commit_ShouldPersistChanges()
    {
        // Arrange
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;

        // Act
        // Start a transaction
        await _unitOfWork.BeginTransactionAsync();

        // Make some changes
        _context.Entities1.Add(entity);

        // Commit the transaction
        _unitOfWork.CommitTransaction();

        // Save changes
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var retrievedEntity = await _context.Entities1.FindAsync(entity.Id);
        Assert.NotNull(retrievedEntity);
        Assert.Equal(entity, retrievedEntity);
    }

    [Fact]
    public void Constructor_NullContext_ThrowsArgumentNullException()
    {
        // Arrange
        AppDbContext nullContext = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UnitOfWork(nullContext));
    }

    [Fact]
    public async Task Transaction_Rollback_ShouldDiscardChanges()
    {
        // Arrange
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;

        // Act
        // Start a transaction
        await _unitOfWork.BeginTransactionAsync();

        // Make some changes
        _context.Entities1.Add(entity);

        await _unitOfWork.SaveChangesAsync();

        // Rollback the transaction
        _unitOfWork.RollbackTransaction();

        // Detach the entity so it's no longer tracked by the DbContext
        _context.Entry(entity).State = EntityState.Detached;

        // Assert
        var retrievedEntity = await _context.Entities1.FindAsync(entity.Id);
        Assert.Null(retrievedEntity);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Database.CloseConnection();
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
