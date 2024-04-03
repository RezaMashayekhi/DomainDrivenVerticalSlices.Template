namespace DomainDrivenVerticalSlices.Template.Application.Interfaces;

public interface IWriteRepository<T>
    where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
