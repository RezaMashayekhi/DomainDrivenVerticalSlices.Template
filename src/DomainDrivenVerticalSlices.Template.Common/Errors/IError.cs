namespace DomainDrivenVerticalSlices.Template.Common.Errors;

using DomainDrivenVerticalSlices.Template.Common.Enums;

public interface IError
{
    string ErrorMessage { get; }

    ErrorType ErrorType { get; }
}
