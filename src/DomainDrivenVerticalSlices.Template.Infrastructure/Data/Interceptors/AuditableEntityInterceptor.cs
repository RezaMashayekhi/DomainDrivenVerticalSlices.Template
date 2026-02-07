namespace DomainDrivenVerticalSlices.Template.Infrastructure.Data.Interceptors;

using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

/// <summary>
/// EF Core interceptor that automatically sets audit properties (Created, CreatedBy, LastModified, LastModifiedBy)
/// on entities that inherit from BaseAuditableEntity.
/// </summary>
public class AuditableEntityInterceptor(
    IUser user,
    TimeProvider timeProvider) : SaveChangesInterceptor
{
    private readonly IUser _user = user;
    private readonly TimeProvider _timeProvider = timeProvider;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var utcNow = _timeProvider.GetUtcNow();

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _user.Id;
                    entry.Entity.Created = utcNow;
                }

                entry.Entity.LastModifiedBy = _user.Id;
                entry.Entity.LastModified = utcNow;
            }
        }
    }
}
