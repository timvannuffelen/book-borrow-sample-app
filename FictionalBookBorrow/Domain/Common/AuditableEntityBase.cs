namespace Domain.Common;

/// <summary>
/// An base class that acts as the base class for each auditable entity.
/// </summary>
/// <typeparam name="TId">
/// The type to use for the ID of the auditable entity (e.g. <see cref="int"/>, <see cref="string"/> or
/// <see cref="Guid"/>).
/// </typeparam>
/// <inheritdoc cref="EntityBase{TId}"/>
public abstract class AuditableEntityBase<TId> : EntityBase<TId>
{
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = "unknow";
    public DateTime? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
