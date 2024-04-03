namespace DomainDrivenVerticalSlices.Template.Common.Models;

public abstract class ValueObject<T>
    where T : ValueObject<T>
{
#pragma warning disable S3875 // Required for DDD value object equality.
    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
#pragma warning restore S3875
    {
        if (a is null && b is null)
        {
            return true;
        }

        return !(a is null || b is null) && a.Equals(b);
    }

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var valueObject = (ValueObject<T>)obj;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return (current * 23) + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}
