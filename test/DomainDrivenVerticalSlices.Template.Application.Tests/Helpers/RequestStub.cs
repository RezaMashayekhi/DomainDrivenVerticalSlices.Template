namespace DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;

using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

#pragma warning disable SA1402 // File may only contain a single type
public class RequestStub(string testProp1, string testProp2) : IRequest<Unit>
{
    public string TestProp1 { get; set; } = testProp1;

    public string TestProp2 { get; set; } = testProp2;
}

public class WithSensitiveAndDateTimePropertiesRequestStub(
    string testProp1,
    DateTime testProp2,
    string password) : IRequest<Unit>
{
    public string TestProp1 { get; set; } = testProp1;

    public DateTime TestProp2 { get; set; } = testProp2;

    public string Password { get; set; } = password;
}

public class EmptyRequestStub : IRequest<Unit>
{
}

public class ResultRequestStub : IRequest<IResult>
{
}

public class ValueResultRequestStub<T> : IRequest<IResult<T>>
#pragma warning restore SA1402 // File may only contain a single type
{
}
