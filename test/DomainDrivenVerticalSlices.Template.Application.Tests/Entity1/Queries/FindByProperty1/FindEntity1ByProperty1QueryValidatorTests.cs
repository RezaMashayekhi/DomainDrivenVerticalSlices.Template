namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.FindByProperty1;

using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;
using FluentValidation.TestHelper;

public class FindEntity1ByProperty1QueryValidatorTests
{
    private readonly FindEntity1ByProperty1QueryValidator _validator;

    public FindEntity1ByProperty1QueryValidatorTests()
    {
        _validator = new FindEntity1ByProperty1QueryValidator();
    }

    [Fact]
    public void PropertyIsEmpty_ShouldHaveValidationError()
    {
        var query = new FindEntity1ByProperty1Query(string.Empty);
        var result = _validator.TestValidate(query);
        result
            .ShouldHaveValidationErrorFor(x => x.Property1)
            .WithErrorMessage("'Property1' must not be empty.");
    }

    [Fact]
    public void PropertyIsValid_ShouldHaveNoValidationError()
    {
        var query = new FindEntity1ByProperty1Query("ValidString");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Property1);
    }
}
