namespace DomainDrivenVerticalSlices.Template.Api.Common.Errors;

using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

public static class ResultExtensions
{
    public static IResult Match(
        this Result result,
        Func<IResult> onSuccess,
        Func<Error, IResult> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static IResult Match<TValue>(
        this Result<TValue> result,
        Func<TValue, IResult> onSuccess,
        Func<Error, IResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }

    public static IResult ToProblemDetailsResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => Results.ValidationProblem(error.ValidationErrors ?? EmptyValidationErrors()),
            ErrorType.NotFound => Results.Problem(CreateProblemDetails(error, StatusCodes.Status404NotFound)),
            ErrorType.Conflict => Results.Problem(CreateProblemDetails(error, StatusCodes.Status409Conflict)),
            _ => Results.Problem(CreateProblemDetails(error, StatusCodes.Status500InternalServerError)),
        };
    }

    public static Error ToValidationError(this ValidationResult validationResult)
    {
        Dictionary<string, string[]> validationErrors = validationResult.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).Distinct().ToArray());

        return Error.Validation(validationErrors);
    }

    private static ProblemDetails CreateProblemDetails(Error error, int statusCode)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = error.Code,
            Detail = error.Description,
        };
    }

    private static Dictionary<string, string[]> EmptyValidationErrors()
    {
        return [];
    }
}
