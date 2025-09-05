namespace DomainDrivenVerticalSlices.Template.Infrastructure.Tests.Data;

using DomainDrivenVerticalSlices.Template.Domain.Entities;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class Entity1RepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly Entity1Repository _repository;
    private readonly UnitOfWork _unitOfWork;
    private bool _disposed = false;

    public Entity1RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:;")
            .Options;
        _context = new AppDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _repository = new Entity1Repository(_context);
        _unitOfWork = new UnitOfWork(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;

        await _repository.AddAsync(entity, CancellationToken.None);
        var retrievedEntity = await _repository.GetByIdAsync(entity.Id);

        Assert.NotNull(retrievedEntity);
        Assert.Equal(entity, retrievedEntity);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        await _repository.AddAsync(entity, CancellationToken.None);

        // Act
        await _repository.DeleteAsync(entity, CancellationToken.None);
        var retrievedEntity = await _repository.GetByIdAsync(entity.Id);

        // Assert
        Assert.Null(retrievedEntity);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnMatchingEntity()
    {
        // Arrange
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        await _repository.AddAsync(entity, CancellationToken.None);

        await _unitOfWork.SaveChangesAsync();

        // Act
        var retrievedEntity = await _repository.FindAsync(e => e.Id == entity.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(retrievedEntity);
        Assert.Equal(entity, retrievedEntity);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        var entity2 = Entity1.Create(ValueObject1.Create("Property2").Value).Value;
        await _repository.AddAsync(entity1, CancellationToken.None);
        await _repository.AddAsync(entity2, CancellationToken.None);

        await _unitOfWork.SaveChangesAsync();

        // Act
        var entities = await _repository.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(entities);
        Assert.Equal(2, entities.Count());

        Assert.Contains(entity1, entities);
        Assert.Contains(entity2, entities);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntityById()
    {
        // Arrange
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        await _repository.AddAsync(entity, CancellationToken.None);

        // Act
        var retrievedEntity = await _repository.GetByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(retrievedEntity);
        Assert.Equal(entity, retrievedEntity);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnEntitiesMatchingPredicate()
    {
        // Arrange
        var entity1 = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        var entity2 = Entity1.Create(ValueObject1.Create("Property2").Value).Value;
        var entity3 = Entity1.Create(ValueObject1.Create("Property2_Extended").Value).Value;
        await _repository.AddAsync(entity1, CancellationToken.None);
        await _repository.AddAsync(entity2, CancellationToken.None);
        await _repository.AddAsync(entity3, CancellationToken.None);

        await _unitOfWork.SaveChangesAsync();

        // Act
        var entities = await _repository.ListAsync(e => e.ValueObject1.Property1.Contains("Property2"), CancellationToken.None);

        // Assert
        Assert.NotNull(entities);
        Assert.Equal(2, entities.Count());

        Assert.Contains(entities, e => e.Id == entity2.Id);
        Assert.Contains(entities, e => e.Id == entity3.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entity = Entity1.Create(ValueObject1.Create("Property1").Value).Value;
        await _repository.AddAsync(entity, CancellationToken.None);

        // Modify the entity
        entity.Update(ValueObject1.Create("Property1_Edited").Value);

        // Act
        await _repository.UpdateAsync(entity, CancellationToken.None);
        var retrievedEntity = await _context.Entities1.FindAsync(entity.Id);

        // Assert
        Assert.NotNull(retrievedEntity);
        Assert.Equal(entity, retrievedEntity);
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
