namespace DomainDrivenVerticalSlices.Template.Common.Tests.Errors;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;

public class ErrorTests
{
    [Fact]
    public void Create_Error_ReturnsCorrectValues()
    {
        // Arrange
        var errorType = ErrorType.NotFound;
        var errorMessage = "Resource not found.";

        // Act
        var error = Error.Create(errorType, errorMessage);

        // Assert
        error.ErrorType.Should().Be(errorType);
        error.ErrorMessage.Should().Be(errorMessage);
    }

    [Theory]
    [InlineData(ErrorType.None, "No error.")]
    [InlineData(ErrorType.NotFound, "Resource not found.")]
    [InlineData(ErrorType.InvalidInput, "Invalid input provided.")]
    [InlineData(ErrorType.OperationFailed, "Failed Operation.")]
    [InlineData((ErrorType)100, "Unknown error.")]
    public void Create_ErrorWithDefaultMessage_ReturnsCorrectDefaultErrorMessage(ErrorType errorType, string expectedErrorMessage)
    {
        // Act
        var error = Error.Create(errorType);

        // Assert
        error.ErrorType.Should().Be(errorType);
        error.ErrorMessage.Should().Be(expectedErrorMessage);
    }
}
