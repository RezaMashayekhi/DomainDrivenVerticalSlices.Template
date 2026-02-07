namespace DomainDrivenVerticalSlices.Template.Infrastructure.Data.Interceptors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

/// <summary>
/// Extension methods for EntityEntry.
/// </summary>
public static class EntityEntryExtensions
{
    /// <summary>
    /// Checks if any owned entities have been modified.
    /// </summary>
    /// <param name="entry">The entity entry to check.</param>
    /// <returns>True if any owned entities have been added or modified; otherwise, false.</returns>
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }
}
