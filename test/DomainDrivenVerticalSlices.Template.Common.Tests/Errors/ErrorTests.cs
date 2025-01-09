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
        error.ErrorMessages.Should().Contain(errorMessage);
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
        error.ErrorMessages.Should().Contain(expectedErrorMessage);
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
        error.ErrorType.Should().Be(errorType);
        error.ErrorMessage.Should().Be("Resource not found., Another message.");
        error.ErrorMessages.Should().BeEquivalentTo(errorMessages);
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
        error.ErrorType.Should().Be(errorType);
        error.ErrorMessage.Should().Be("Resource not found.");
        error.ErrorMessages.Should().Contain("Resource not found.");
    }
}
