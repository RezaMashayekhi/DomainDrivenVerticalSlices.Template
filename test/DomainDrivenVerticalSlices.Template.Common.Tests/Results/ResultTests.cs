namespace DomainDrivenVerticalSlices.Template.Common.Tests.Results;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;

public class ResultTests
{
    private const string ErrorMessage = "error message";
    private readonly IError _error = Error.Create(ErrorType.InvalidInput, ErrorMessage);

    [Fact]
    public void CheckedError_ReturnsError_WhenFailureResult()
    {
        // Arrange
        var result = Result.Failure(_error);

        // Act & Assert
        Assert.Equal(_error, result.CheckedError);
    }

    [Fact]
    public void CheckedError_ThrowsException_WhenSuccessResult()
    {
        // Arrange
        var result = Result.Success();

        // Act
        Action act = () => { _ = result.CheckedError; };

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Contains("Attempted to access CheckedError property when no error is set.", exception.Message);
    }

    [Fact]
    public void Error_ReturnsError_WhenFailureResult()
    {
        // Arrange
        var result = Result.Failure(_error);

        // Act & Assert
        Assert.Equal(_error, result.Error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult()
    {
        // Arrange & Act
        var result = Result.Failure(_error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(_error, result.Error);
    }

    [Fact]
    public void Failure_ThrowsException_WhenErrorIsNull()
    {
        // Arrange & Act
        Action act = () => { _ = Result.Failure(null!); };

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Contains("error", exception.Message);
    }

    [Fact]
    public void IsSuccess_ReturnsFalse_WhenFailureResult()
    {
        // Arrange
        var result = Result.Failure(_error);

        // Act & Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_ReturnsTrue_WhenSuccessResult()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Success_ReturnsSuccessResult()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }
}
