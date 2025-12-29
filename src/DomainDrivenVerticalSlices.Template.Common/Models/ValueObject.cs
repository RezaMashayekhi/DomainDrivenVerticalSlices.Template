namespace DomainDrivenVerticalSlices.Template.Common.Models;

public abstract class ValueObject<T>
    where T : ValueObject<T>
{
    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
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
        HashCode hashCode = default;
        foreach (var component in GetEqualityComponents())
        {
            hashCode.Add(component);
        }

        return hashCode.ToHashCode();
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}
