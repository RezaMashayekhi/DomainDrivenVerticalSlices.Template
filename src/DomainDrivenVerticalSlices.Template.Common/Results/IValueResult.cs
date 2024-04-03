namespace DomainDrivenVerticalSlices.Template.Common.Results;

#pragma warning disable SA1649 // File name should match first type name
public interface IResult<out TValue> : IResult
#pragma warning restore SA1649 // File name should match first type name
{
    TValue Value { get; }
}
