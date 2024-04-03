namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Commands.Create;

using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Create;
using FluentValidation.TestHelper;

public class CreateEntity1CommandValidatorTests
{
    private readonly CreateEntity1CommandValidator _validator;

    public CreateEntity1CommandValidatorTests()
    {
        _validator = new CreateEntity1CommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    public void Property1_ShouldHaveValidationError_WhenEmpty(string property1)
    {
        var command = new CreateEntity1Command(property1);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Property1);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("Some Value")]
    public void Property1_ShouldNotHaveValidationError_WhenNotEmpty(string property1)
    {
        var command = new CreateEntity1Command(property1);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Property1);
    }
}
