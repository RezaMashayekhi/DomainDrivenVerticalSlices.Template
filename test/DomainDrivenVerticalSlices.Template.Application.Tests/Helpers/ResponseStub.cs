namespace DomainDrivenVerticalSlices.Template.Application.Tests.Helpers;

public class ResponseStub(string prop1, string password)
{
    public string Prop1 { get; set; } = prop1;

    public string Password { get; set; } = password;
}
