namespace Domain.Common;

/// <summary>
/// An base class that acts as the base class for each entity.
/// </summary>
/// <typeparam name="TId">
/// The type to use for the ID of the entity (e.g. <see cref="int"/>, <see cref="string"/> or <see cref="Guid"/>).
/// </typeparam>
public abstract class EntityBase<TId>
{
    public TId Id { get; set; }
}
