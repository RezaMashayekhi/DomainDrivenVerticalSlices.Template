namespace DomainDrivenVerticalSlices.Template.Common.Errors;

using DomainDrivenVerticalSlices.Template.Common.Enums;

public class Error : IError
{
    private Error(ErrorType errorType, IEnumerable<string> errorMessages)
    {
        ErrorType = errorType;
        ErrorMessages = errorMessages;
    }

    public ErrorType ErrorType { get; private set; }

    public IEnumerable<string> ErrorMessages { get; private set; }

    public string ErrorMessage
    {
        get { return string.Join(", ", ErrorMessages); }
    }

    public static Error Create(ErrorType errorType, string errorMessage = "")
    {
        IEnumerable<string> errorMessages = string.IsNullOrWhiteSpace(errorMessage)
            ? GetDefaultErrorMessages(errorType)
            : [errorMessage];
        return new Error(errorType, errorMessages);
    }

    public static Error Create(ErrorType errorType, IEnumerable<string> errorMessages)
    {
        if (!errorMessages.Any())
        {
            errorMessages = GetDefaultErrorMessages(errorType);
        }

        return new Error(errorType, errorMessages);
    }

    private static IEnumerable<string> GetDefaultErrorMessages(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.None => ["No error."],
            ErrorType.NotFound => ["Resource not found."],
            ErrorType.InvalidInput => ["Invalid input provided."],
            ErrorType.OperationFailed => ["Failed Operation."],
            _ => ["Unknown error."],
        };
    }
}
