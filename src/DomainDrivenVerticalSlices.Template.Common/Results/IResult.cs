namespace DomainDrivenVerticalSlices.Template.Common.Results;

using DomainDrivenVerticalSlices.Template.Common.Errors;

public interface IResult
{
    bool IsSuccess { get; }

    IError? Error { get; }

    IError CheckedError { get; }
}
