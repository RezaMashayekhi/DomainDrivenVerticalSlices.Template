namespace DomainDrivenVerticalSlices.Template.Domain.Entities;

/// <summary>
/// Base class for entities that require audit trail information.
/// Extends BaseEntity with Created/Modified timestamps and user tracking.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the UTC timestamp when the entity was created.
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the entity was last modified.
    /// </summary>
    public DateTimeOffset LastModified { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the entity.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}
