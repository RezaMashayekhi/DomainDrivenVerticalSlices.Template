namespace DomainDrivenVerticalSlices.Template.Domain.ValueObjects;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Models;
using DomainDrivenVerticalSlices.Template.Common.Results;

public class ValueObject1 : ValueObject<ValueObject1>
{
    private ValueObject1(string property1)
    {
        Property1 = property1;
    }

    public string Property1 { get; private set; }

    public static Result<ValueObject1> Create(string property1)
    {
        if (string.IsNullOrWhiteSpace(property1))
        {
            var error = Error.Create(ErrorType.InvalidInput, $"{nameof(property1)} cannot be empty.");
            return Result<ValueObject1>.Failure(error);
        }

        return Result<ValueObject1>.Success(new ValueObject1(property1));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Property1;
    }
}
