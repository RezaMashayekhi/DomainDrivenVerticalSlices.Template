namespace DomainDrivenVerticalSlices.Template.Common.Errors;

using DomainDrivenVerticalSlices.Template.Common.Enums;

public class Error : IError
{
    private Error(ErrorType errorType, string errorMessage)
    {
        ErrorType = errorType;
        ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? GetDefaultErrorMessage(errorType) : errorMessage;
    }

    public ErrorType ErrorType { get; private set; }

    public string ErrorMessage { get; private set; }

    public static Error Create(ErrorType errorType, string errorMessage = "")
    {
        return new Error(errorType, errorMessage);
    }

    private static string GetDefaultErrorMessage(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.None => "No error.",
            ErrorType.NotFound => "Resource not found.",
            ErrorType.InvalidInput => "Invalid input provided.",
            ErrorType.OperationFailed => "Failed Operation.",
            _ => "Unknown error.",
        };
    }
}
