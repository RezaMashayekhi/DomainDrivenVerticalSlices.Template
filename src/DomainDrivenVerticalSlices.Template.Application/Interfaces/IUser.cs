namespace DomainDrivenVerticalSlices.Template.Application.Interfaces;

/// <summary>
/// Represents the current user context.
/// Used for audit trail (CreatedBy/LastModifiedBy).
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// Returns null if no user is authenticated.
    /// </summary>
    string? Id { get; }
}
