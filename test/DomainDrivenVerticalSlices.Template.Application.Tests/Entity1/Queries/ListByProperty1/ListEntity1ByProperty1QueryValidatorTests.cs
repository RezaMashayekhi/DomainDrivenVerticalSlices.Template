namespace DomainDrivenVerticalSlices.Template.Application.Tests.Entity1.Queries.ListByProperty1;

using DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;
using FluentValidation.TestHelper;

public class ListEntity1ByProperty1QueryValidatorTests
{
    private readonly ListEntity1ByProperty1QueryValidator _validator;

    public ListEntity1ByProperty1QueryValidatorTests()
    {
        _validator = new ListEntity1ByProperty1QueryValidator();
    }

    [Fact]
    public void PropertyIsEmpty_ShouldHaveValidationError()
    {
        var query = new ListEntity1ByProperty1Query(string.Empty);
        var result = _validator.TestValidate(query);
        result
            .ShouldHaveValidationErrorFor(x => x.Property1)
            .WithErrorMessage("'Property1' must not be empty.");
    }

    [Fact]
    public void PropertyIsValid_ShouldHaveNoValidationError()
    {
        var query = new ListEntity1ByProperty1Query("Valid Property");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Property1);
    }
}
