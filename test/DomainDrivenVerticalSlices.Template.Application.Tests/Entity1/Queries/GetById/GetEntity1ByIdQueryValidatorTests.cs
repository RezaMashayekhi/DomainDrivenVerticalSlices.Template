namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.GetById;

using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;
using FluentValidation.TestHelper;

public class GetEntity1ByIdQueryValidatorTests
{
    private readonly GetEntity1ByIdQueryValidator _validator;

    public GetEntity1ByIdQueryValidatorTests()
    {
        _validator = new GetEntity1ByIdQueryValidator();
    }

    [Fact]
    public void Id_ShouldHaveValidationError_WithCorrectMessage_WhenNotValid()
    {
        var query = new GetEntity1ByIdQuery(Guid.Empty);
        var result = _validator.TestValidate(query);
        result
            .ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("'Id' must not be empty.");
    }

    [Fact]
    public void Id_ShouldNotHaveValidationError_WhenValid()
    {
        var validGuid = Guid.NewGuid();
        var query = new GetEntity1ByIdQuery(validGuid);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
