namespace DomainDrivenVerticalSlices.Template.Common.Results;

using DomainDrivenVerticalSlices.Template.Common.Errors;

public class Result : IResult
{
    protected Result(bool isSuccess, IError? error)
    {
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

    public static Result Success()
    {
        return new Result(true, default);
    }

    public static Result Failure(IError error)
    {
        return error == null ? throw new ArgumentNullException(nameof(error)) : new Result(false, error);
    }
}
