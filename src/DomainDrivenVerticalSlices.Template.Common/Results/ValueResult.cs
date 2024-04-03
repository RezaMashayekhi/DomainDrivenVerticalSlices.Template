namespace DomainDrivenVerticalSlices.Template.Common.Results;

using DomainDrivenVerticalSlices.Template.Common.Errors;

#pragma warning disable SA1649 // File name should match first type name
public class Result<TValue> : IResult<TValue>
#pragma warning restore SA1649 // File name should match first type name
{
    private readonly TValue? _value;

    protected Result(bool isSuccess, IError? error, TValue? value)
    {
        if (isSuccess && (value == null || value.Equals(default(TValue))))
        {
            throw new ArgumentException("Value cannot be null or empty for a successful result.", nameof(value));
        }

        if (isSuccess && !EqualityComparer<IError>.Default.Equals(error, default))
        {
            throw new ArgumentException("Cannot set error for successful result.", nameof(error));
        }

        if (!isSuccess && EqualityComparer<IError>.Default.Equals(error, default))
        {
            throw new ArgumentException("Must set error for failed result.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        _value = value;
    }

    public bool IsSuccess { get; }

    public IError? Error { get; }

    public IError CheckedError
    {
        get
        {
            return Error ?? throw new InvalidOperationException("Attempted to access CheckedError property when no error is set.");
        }
    }

    public TValue Value
    {
        get
        {
            return _value ?? throw new InvalidDataException("Cannot retrieve value from a failed result.");
        }
    }

    public static Result<TValue> Success(TValue value)
    {
        return value == null ? throw new ArgumentNullException(nameof(value)) : new Result<TValue>(true, default, value);
    }

    public static Result<TValue> Failure(IError error)
    {
        return error == null ? throw new ArgumentNullException(nameof(error)) : new Result<TValue>(false, error, default);
    }
}
