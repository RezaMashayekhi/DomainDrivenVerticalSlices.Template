namespace DomainDrivenVerticalSlices.Template.Infrastructure.Data;

using System.Linq.Expressions;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class Entity1Repository(AppDbContext context) : IEntity1Repository
{
    private readonly AppDbContext _context = context;

    public async Task<Entity1> AddAsync(Entity1 entity, CancellationToken cancellationToken = default)
    {
        await _context.Entities1.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Entity1 entity, CancellationToken cancellationToken = default)
    {
        _context.Entities1.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<Entity1?> FindAsync(Expression<Func<Entity1, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Entities1.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Entity1>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Entities1.ToListAsync(cancellationToken);
    }

    public async Task<Entity1?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : notnull
    {
        return await _context.Entities1.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Entity1>> ListAsync(Expression<Func<Entity1, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Entities1.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Entity1 entity, CancellationToken cancellationToken = default)
    {
        _context.Entities1.Update(entity);
        await Task.CompletedTask;
    }
}
