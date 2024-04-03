namespace DomainDrivenVerticalSlices.Template.IntegrationTests.Helpers;

using DomainDrivenVerticalSlices.Template.Common.Enums;

public class ErrorDto
{
    public ErrorType ErrorType { get; set; }

    public string? ErrorMessage { get; set; }
}
