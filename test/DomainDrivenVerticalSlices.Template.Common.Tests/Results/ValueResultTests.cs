namespace DomainDrivenVerticalSlices.Template.Common.Tests.Results;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;

public class ValueResultTests
{
    private const string ErrorMessage = "error message";
    private const string TestValue = "Test Value Text";
    private readonly IError _error = Error.Create(ErrorType.InvalidInput, ErrorMessage);

    [Fact]
    public void CheckedError_ReturnsErrorMessage_WhenFailureResult()
    {
        // Arrange
        var result = Result<int>.Failure(_error);

        // Act & Assert
        Assert.Equal(_error, result.CheckedError);
    }

    [Fact]
    public void CheckedError_ThrowsException_WhenValueResultSuccess()
    {
        // Arrange
        var result = Result<string>.Success(TestValue);

        // Act
        Action act = () => { _ = result.CheckedError; };

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Contains("Attempted to access CheckedError property when no error is set.", exception.Message);
    }

    [Fact]
    public void Error_ReturnsErrorMessage_WhenFailureResult()
    {
        // Arrange
        var result = Result<string>.Failure(_error);

        // Act & Assert
        Assert.Equal(_error, result.Error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult()
    {
        // Arrange & Act
        var result = Result<string>.Failure(_error);
        Action act = () => { _ = result.Value; };

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(_error, result.Error);
        var exception = Assert.Throws<InvalidDataException>(act);
        Assert.Contains("Cannot retrieve value from a failed result.", exception.Message);
    }

    [Fact]
    public void Failure_WithNullError_ThrowsException()
    {
        // Arrange & Act
        Action act = () => Result<string>.Failure(null!);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Contains("error", exception.Message);
    }

    [Fact]
    public void IsSuccess_ReturnsFalse_WhenFailureResult()
    {
        // Arrange
        var result = Result<string>.Failure(_error);

        // Act & Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_ReturnsTrue_WhenSuccessResult()
    {
        // Arrange
        var result = Result<string>.Success(TestValue);

        // Act & Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Success_ReturnsSuccessResultWithValue()
    {
        // Arrange & Act
        var result = Result<string>.Success(TestValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
        Assert.Equal(TestValue, result.Value);
    }

    [Fact]
    public void Success_WithNullValue_ThrowsException()
    {
        // Arrange & Act
        Action act = () => Result<string>.Success(null!);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Contains("value", exception.Message);
    }

    [Fact]
    public void Success_WithDefaultValue_ThrowsException()
    {
        Action act = () => Result<int>.Success(default);

        var exception = Assert.Throws<ArgumentException>(act);
        Assert.Contains("Value cannot be null or empty for a successful result.", exception.Message);
    }

    [Fact]
    public void Value_ReturnsValue_WhenSuccessResult()
    {
        // Arrange
        var result = Result<string>.Success(TestValue);

        // Act & Assert
        Assert.Equal(TestValue, result.Value);
    }

    [Fact]
    public void Value_ThrowsException_WhenFailureResult()
    {
        // Arrange
        var result = Result<string>.Failure(_error);

        // Act
        Action act = () => { _ = result.Value; };

        // Assert
        var exception = Assert.Throws<InvalidDataException>(act);
        Assert.Contains("Cannot retrieve value from a failed result.", exception.Message);
    }
}
