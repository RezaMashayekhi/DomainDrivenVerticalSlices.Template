namespace DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;

using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;

#pragma warning disable SA1402 // File may only contain a single type
public class RequestStub(string testProp1, string testProp2) : ICommand<Unit>
{
    public string TestProp1 { get; set; } = testProp1;

    public string TestProp2 { get; set; } = testProp2;
}

public class WithSensitiveAndDateTimePropertiesRequestStub(
    string testProp1,
    DateTime testProp2,
    string password) : ICommand<Unit>
{
    public string TestProp1 { get; set; } = testProp1;

    public DateTime TestProp2 { get; set; } = testProp2;

    public string Password { get; set; } = password;
}

public class EmptyRequestStub : ICommand<Unit>
{
}

public class ResultRequestStub : ICommand<IResult>
{
}

public class ValueResultRequestStub<T> : ICommand<IResult<T>>
#pragma warning restore SA1402 // File may only contain a single type
{
}
