namespace DomainDrivenVerticalSlices.Template.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    void CommitTransaction();

    void RollbackTransaction();
}
