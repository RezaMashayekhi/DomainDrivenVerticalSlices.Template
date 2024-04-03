namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Commands.Update;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;
using FluentValidation.TestHelper;

public class UpdateEntity1CommandValidatorTests
{
    private readonly UpdateEntity1CommandValidator _validator;

    public UpdateEntity1CommandValidatorTests()
    {
        _validator = new UpdateEntity1CommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    public void Property1_ShouldHaveValidationError_WhenEmpty(string property1)
    {
        var id = Guid.NewGuid();
        var valueObject1 = new ValueObject1Dto(property1);
        var entity1Dto = new Entity1Dto(id, valueObject1);
        var command = new UpdateEntity1Command(entity1Dto);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Entity1.ValueObject1.Property1);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("Some Value")]
    public void Property1_ShouldNotHaveValidationError_WhenNotEmpty(string property1)
    {
        var id = Guid.NewGuid();
        var valueObject1 = new ValueObject1Dto(property1);
        var entity1Dto = new Entity1Dto(id, valueObject1);
        var command = new UpdateEntity1Command(entity1Dto);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Entity1.ValueObject1.Property1);
    }
}
