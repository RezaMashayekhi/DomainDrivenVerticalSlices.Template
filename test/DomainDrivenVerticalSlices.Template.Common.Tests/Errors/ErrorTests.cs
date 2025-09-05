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
        Assert.Equal(errorType, error.ErrorType);
        Assert.Equal(errorMessage, error.ErrorMessage);
        Assert.Contains(errorMessage, error.ErrorMessages);
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
        Assert.Equal(errorType, error.ErrorType);
        Assert.Equal(expectedErrorMessage, error.ErrorMessage);
        Assert.Contains(expectedErrorMessage, error.ErrorMessages);
    }

    [Fact]
    public void Create_ErrorWithList_ReturnsCorrectValues()
    {
        // Arrange
        var errorType = ErrorType.NotFound;
        var errorMessages = new List<string> { "Resource not found.", "Another message." };

        // Act
        var error = Error.Create(errorType, errorMessages);

        // Assert
        Assert.Equal(errorType, error.ErrorType);
        Assert.Equal("Resource not found., Another message.", error.ErrorMessage);
        Assert.Equal(errorMessages, error.ErrorMessages);
    }

    [Fact]
    public void Create_ErrorWithEmptyList_ReturnsCorrectDefaultErrorMessage()
    {
        // Arrange
        var errorType = ErrorType.NotFound;
        var errorMessages = new List<string>();

        // Act
        var error = Error.Create(errorType, errorMessages);

        // Assert
        Assert.Equal(errorType, error.ErrorType);
        Assert.Equal("Resource not found.", error.ErrorMessage);
        Assert.Contains("Resource not found.", error.ErrorMessages);
    }
}
