namespace DomainDrivenVerticalSlices.Template.Api.Common.Errors;

public sealed record Error(
    string Code,
    string Description,
    ErrorType Type,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    public static Error Validation(IReadOnlyDictionary<string, string[]> validationErrors)
    {
        return new Error(
            "Validation.Failed",
            "One or more validation errors occurred.",
            ErrorType.Validation,
            validationErrors);
    }

    public static Error NotFound(string code, string description)
    {
        return new Error(code, description, ErrorType.NotFound);
    }

    public static Error Conflict(string code, string description)
    {
        return new Error(code, description, ErrorType.Conflict);
    }

    public static Error Failure(string code, string description)
    {
        return new Error(code, description, ErrorType.Failure);
    }
}
