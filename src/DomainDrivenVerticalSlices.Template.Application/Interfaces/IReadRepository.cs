namespace DomainDrivenVerticalSlices.Template.Application.Interfaces;

using System.Linq.Expressions;

public interface IReadRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : notnull;

    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}
