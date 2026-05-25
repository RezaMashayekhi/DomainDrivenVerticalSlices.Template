namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;

public sealed class ValueObject1 : IEquatable<ValueObject1>
{
    private ValueObject1()
    {
    }

    private ValueObject1(string property1)
    {
        Property1 = property1.Trim();
    }

    public string Property1 { get; private set; } = string.Empty;

    public static Result<ValueObject1> Create(string property1)
    {
        if (string.IsNullOrWhiteSpace(property1))
        {
            return Result<ValueObject1>.Failure(Entity1Errors.Property1Required());
        }

        return new ValueObject1(property1);
    }

    public bool Equals(ValueObject1? other)
    {
        return other is not null &&
            string.Equals(Property1, other.Property1, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueObject1);
    }

    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode(Property1);
    }
}
