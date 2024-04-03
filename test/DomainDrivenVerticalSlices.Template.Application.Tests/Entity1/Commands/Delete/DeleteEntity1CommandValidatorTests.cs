namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Commands.Delete;

using DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;
using FluentValidation.TestHelper;

public class DeleteEntity1CommandValidatorTests
{
    private readonly DeleteEntity1CommandValidator _validator;

    public DeleteEntity1CommandValidatorTests()
    {
        _validator = new DeleteEntity1CommandValidator();
    }

    [Fact]
    public void Id_ShouldHaveValidationError_WhenEmpty()
    {
        var id = Guid.Empty;
        var command = new DeleteEntity1Command(id);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Id_ShouldNotHaveValidationError_WhenIsGuid()
    {
        var id = Guid.NewGuid();
        var command = new DeleteEntity1Command(id);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
