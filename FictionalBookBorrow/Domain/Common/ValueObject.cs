namespace Domain.Common;

/// <summary>
/// An abstract class that acts as the base class of all value objects.
/// </summary>
/// <remarks>
/// Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
/// </remarks>
public abstract class ValueObject
{
    /// <summary>
    /// Returns the components that are used to define the identity of a value object. Two value objects are considered 
    /// equal when they have the same value for the returned components. The value of components (properties) that are 
    /// <em>not</em> returned are ignored when determining whether or not two value objects are equal.
    /// </summary>
    /// <returns>
    /// All components/properties that, when combined, define the "identity" of the value object.
    /// </returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Checks whether or not two specified value objects are equal.
    /// </summary>
    /// <param name="left">
    /// A value object.
    /// </param>
    /// <param name="right">
    /// Another value object.
    /// </param>
    /// <returns>
    /// True if and only if the specified value objects are equal, false otherwise.
    /// </returns>
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        // Recap: Here's the truth table for the XOR operator:
        //
        // | left | right | result |
        // |------|-------|--------|
        // |   0  |   0   |    0   |
        // |   0  |   1   |    1   |
        // |   1  |   0   |    1   |
        // |   1  |   1   |    0   |

        if (left is null ^ right is null)
        {
            // When one value object is null and the other is not they are, of course, not equal.
            return false;
        }

        return ReferenceEquals(left, right) || left.Equals(right);

        // When we get here and "left" is not null, then "right" is also not null. That's why we can use the null-forgiving
        // operator (!) here. When "left" is null, "right" is not evaluated because of the short-circuiting null-conditional
        // operator (?.).
        //return left?.Equals(right!) != false;
    }

    /// <summary>
    /// Checks whether or not two specified value objects are <em>not</em> equal.
    /// </summary>
    /// <param name="left">
    /// A value object.
    /// </param>
    /// <param name="right">
    /// Another value object.
    /// </param>
    /// <returns>
    /// True if and only if the specified value objects are <em>not</em> equal, false otherwise.
    /// </returns>
    protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !EqualOperator(left, right);

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : 0)
                                                                .Aggregate((x, y) => x ^ y);
}
