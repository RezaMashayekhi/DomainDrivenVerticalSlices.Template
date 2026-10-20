namespace DomainDrivenVerticalSlices.Template.WebApi.Infrastructure;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;

internal static class ApiErrors
{
    public static (int StatusCode, IError Error) Map(IResult result)
    {
        var error = result.CheckedError;
        return error.ErrorType switch
        {
            ErrorType.NotFound => (StatusCodes.Status404NotFound, error),
            ErrorType.InvalidInput or ErrorType.OperationFailed => (StatusCodes.Status400BadRequest, error),
            _ => (StatusCodes.Status400BadRequest, Error.Create(ErrorType.OperationFailed, "An error occurred while processing the request.")),
        };
    }
}
